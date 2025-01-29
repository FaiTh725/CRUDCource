using Application.Contracts.Response;
using Product.Dal.Interfaces;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Contracts.Models.Request;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Entities;
using Product.Domain.Models;
using Product.Helpers.Settings;
using Product.Services.Interfaces;
using System.Text;
using System.Text.Json;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IChangeRoleRepository changeRoleRepository;
        private readonly IProductRepository productRepository;
        private readonly ICartItemRepository cartItemRepository;
        private readonly IBlobService blobService;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly IDatabaseTransaction databaseTransaction;
        private readonly ITelemetryService telemetryService;
        private readonly ICachService cachService;

        public AccountService(
            IAccountRepository accountRepository,
            IChangeRoleRepository changeRoleRepository,
            IBlobService blobService,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IProductRepository productRepository,
            ICartItemRepository cartItemRepository,
            IDatabaseTransaction databaseTransaction,
            ITelemetryService telemetryService,
            ICachService cachService)
        {
            this.accountRepository = accountRepository;
            this.changeRoleRepository = changeRoleRepository;
            this.productRepository = productRepository;
            this.cartItemRepository = cartItemRepository;
            this.blobService = blobService;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.databaseTransaction = databaseTransaction;
            this.telemetryService = telemetryService;
            this.cachService = cachService;
        }

        public async Task<BaseResponse> AddProductToCart(AccountWithProductCountRequest request)
        {
            try
            {
                var account = await accountRepository.GetAccountWithCart(request.Email);

                if(account.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Accocunt with current email was not found"
                    };
                }

                var productInCart = account.Value.ShopingCart
                    .FirstOrDefault(x => x.Product.Id == request.ProductId);

                if (productInCart != null)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Product already in cart"
                    };
                }

                var product = await productRepository.GetProduct(request.ProductId);

                if(product.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Product not exist"
                    };
                }

                if(product.Value.Count < request.Count || request.Count <= 0)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Count products is too large"
                    };
                }

                var cartItem = CartItem.Initialize(request.Count, product.Value);

                if(cartItem.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = cartItem.Error
                    };
                }

                var resultUpdate = await accountRepository.AddProductsToCart(account.Value, new List<CartItem> { cartItem.Value });

                if(resultUpdate.IsFailure)
                {
                    return new BaseResponse
                    {
                        Description = "Internal Server Error",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                await cachService.RemoveKey($"account-{request.Email}:cartItems");

                return new BaseResponse
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success add product to cart"
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Internal Server Eror",
                    StatusCode = StatusCode.Ok
                };
            }
        }

        public async Task<BaseResponse> BuyProducts(AccountBuyProductRequest request)
        {
            try
            {
                var account = await accountRepository.GetAccountWithCart(request.Email);

                if(account.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = account.Error
                    };
                }
                await databaseTransaction.StartTransaction();

                //var itemsToRemove = account.Value.ShopingCart.Where(x => request.Products.Any(y => y.ProductId == x.Product.Id));
                var itemsToRemove = new List<CartItem>();
                var productsToUpdate = new List<ProductEntity>();

                foreach (var requestProduct in request.Products)
                {
                    var cartItem = account.Value.ShopingCart
                        .FirstOrDefault(x => x.Product.Id == requestProduct.ProductId);

                    if (cartItem == null)
                    {
                        return new BaseResponse 
                        { 
                            StatusCode = StatusCode.BadRequest,
                            Description = "Product is not in cart"
                        };
                    }

                    cartItem.Count = requestProduct.Count;
                    itemsToRemove.Add(cartItem);

                    var resultBuy = cartItem.Product.BuyProduct(requestProduct.Count);

                    if(!resultBuy)
                    {
                        return new BaseResponse 
                        { 
                            Description = "Invalid count product for buy",
                            StatusCode = StatusCode.BadRequest,
                        };

                    }

                    productsToUpdate.Add(cartItem.Product);
                }

                var resultUpdate = await productRepository.UpdateProducts(productsToUpdate);

                var resultRemove = await cartItemRepository
                    .RemoveCarts(itemsToRemove);

                if(resultRemove.IsFailure || resultUpdate.IsFailure)
                {
                    await databaseTransaction.RollBackTransaction();
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = resultRemove.Error
                    };
                }

                var itemsForOrderHistory = itemsToRemove.Select(x => 
                {
                    var newItemCart = CartItem.Initialize(x.Count, x.Product);

                    if(newItemCart.IsFailure)
                    {
                        throw new InvalidDataException();
                    }

                    return newItemCart.Value;
                }).ToList();

                var accountWithShopingHistory = await accountRepository
                    .GetAccountWithOrderHistory(request.Email);

                var resultAddItemsToHistory = await accountRepository
                    .AddProductToOrderHistory(accountWithShopingHistory.Value, itemsForOrderHistory);

                if(resultAddItemsToHistory.IsFailure)
                {
                    await databaseTransaction.RollBackTransaction();
                    return new BaseResponse 
                    { 
                        Description = resultAddItemsToHistory.Error,
                        StatusCode = StatusCode.InternalServerError
                    };

                }

                // create specifics metrics
                foreach(var cartItem in itemsForOrderHistory)
                {
                    telemetryService.RecordProductBought(cartItem.Product.Id, cartItem.Count);
                }


                await databaseTransaction.CommitTransaction();

                await cachService.RemoveKey($"account-{request.Email}:cartItems");
                
                return new BaseResponse
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Product bought"
                };
            }
            catch
            {
                await databaseTransaction.RollBackTransaction();

                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error"
                };
            }
            finally
            {
                await databaseTransaction.Dispose();
            }
        }

        public async Task<BaseResponse> CommitRequest(CommitRequestChangeRole requestChangeRole)
        {
            try
            {
                var request = await changeRoleRepository.GetRequest(requestChangeRole.RequestId);

                if (request.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid request id"
                    };
                }

                if(request.Value.IsReviewed)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Current request is commited"
                    };
                }

                var jsonData = JsonSerializer.Serialize(new ChangeRoleAccount
                {
                    Email = request.Value.Email,
                    NewRole = request.Value.NewRole,
                });

                var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var apiList = configuration.GetSection("APIList").Get<APIList>() ??
                    throw new NullReferenceException("Configuration of api is empty");

                var httpClient = httpClientFactory.CreateClient("ClientHttp");

                var response = await httpClient.PatchAsync(
                    $@"{apiList.AuthorizeAPI}/User/UpdateUser",
                    stringContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Error send request to another api"
                    };
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<BaseResponse>(responseJson)
                    ?? throw new NullReferenceException("Error desialize resposen");

                if (responseData.StatusCode != StatusCode.Ok)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = responseData.Description
                    };
                }

                var resultUpdateRequest = await changeRoleRepository
                    .CompleteRequest(request.Value, requestChangeRole.StatusRequest);

                if (resultUpdateRequest.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Unknown server error while commit request"
                    };
                }

                return new BaseResponse 
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success change role user"
                };

            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Internal Server Error",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse> CreateAccount(CreateAccount account)
        {
            try
            {
                var existAccount = await accountRepository.GetAccountByEmail(account.Email);
            
                if(!existAccount.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Account with email already exist"
                    };
                }

                var newAccount = Account.Initialize(account.Name, account.Email);

                if (newAccount.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = newAccount.Error
                    };
                }

                var resultAdd = await accountRepository.AddAccount(newAccount.Value);

                if(resultAdd.IsSuccess)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "Success create new account"
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Unknown Server error"
                    };
                }
            }
            catch
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internall server error"
                };
            }
        }

        public async Task<BaseResponse> CreateRequestChangeRole(ChangeRoleAccount changeRequest)
        {
            try
            {
                var accountGetRequest = await accountRepository.GetAccountByEmail(changeRequest.Email);

                if(accountGetRequest.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Invalid Email, email with this account not exist"
                    };
                }

                var equalsRequest = await changeRoleRepository.GetUnCommitedRequestUser(changeRequest.Email);

                if (equalsRequest.IsSuccess)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "You already sended request"
                    };
                }

                var newRequestChangeRole = ChangeAccountRoleRequest.Initialize(changeRequest.Email, changeRequest.NewRole);

                if(newRequestChangeRole.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = newRequestChangeRole.Error
                    };
                }

                var resultCreateNewRequest = await changeRoleRepository.CreateRequest(newRequestChangeRole.Value);

                if (resultCreateNewRequest.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = resultCreateNewRequest.Error
                    };
                }

                return new BaseResponse
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success created request to get role permissions"
                };
            }
            catch
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error"
                };
            }
        }

        public async Task<BaseResponse> DeleteProductFromCart(AccountWithProductRequest request)
        {
            try
            {
                var account = await accountRepository.GetAccountWithCart(request.Email);

                if(account.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Request(email value)"
                    };
                }

                var cartItem = account.Value.ShopingCart.FirstOrDefault(x => x.Product.Id == request.ProductId);

                if (cartItem == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Product is not in cart"
                    };
                }

                var resultRemove = await cartItemRepository.RemoveCart(cartItem);

                if(resultRemove.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Unknown Server Error"
                    };
                }

                await cachService.RemoveKey($"account-{request.Email}:cartItems");

                return new BaseResponse
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Product removed from cart"
                };
            }
            catch
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error"
                };
            }
        }

        public async Task<DataResponse<AccountResponseCartItems>> GetAccountCartItems(string email)
        {
            try
            {
                var cachCartItems = await cachService
                    .GetData<AccountResponseCartItems>($"account-{email}:cartItems");
                
                if(cachCartItems.IsSuccess)
                {
                    return new DataResponse<AccountResponseCartItems>
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "",
                        Data = cachCartItems.Value
                    };
                }
                
                var account = await accountRepository.GetAccountWithCart(email);

                if(account.IsFailure)
                {
                    return new DataResponse<AccountResponseCartItems>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = account.Error
                    };
                }

                var cartItems = (await Task.WhenAll(
                    account.Value.ShopingCart.Select(async x => new ProductCartResponse
                    {
                        Id = x.Product.Id,
                        Count = x.Count,
                        Description = x.Product.Description,
                        Name = x.Product.Name,
                        MaxCountProduct = x.Product.Count,
                        Price = x.Product.Price,
                        
                        Images = await blobService.DownLoadBlobFolder(x.Product.ImageFolder)
                    }))).ToList();

                var accountCartItems = new AccountResponseCartItems
                {
                    Email = account.Value.Email,
                    Name = account.Value.Name,
                    CartProducts = cartItems
                };

                await cachService.SetData($"account-{email}:cartItems", accountCartItems, 120);

                return new DataResponse<AccountResponseCartItems>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get account cart items",
                    Data = accountCartItems
                };
            }
            catch
            {
                return new DataResponse<AccountResponseCartItems>
                {
                    Data = new AccountResponseCartItems(),
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error"
                };
            }
        }

        public async Task<DataResponse<AccountResponseDetail>> GetAccountDetails(string email)
        {
            try
            {
                var account = await accountRepository.GetAccountByEmail(email);

                if(account.IsFailure)
                {
                    return new DataResponse<AccountResponseDetail>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = account.Error,
                        Data = new AccountResponseDetail()
                    };
                }

                return new DataResponse<AccountResponseDetail>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success get account info",
                    Data = new AccountResponseDetail()
                    {
                        Email = account.Value.Email,
                        Name = account.Value.Name
                    }
                };
            }
            catch
            {
                return new DataResponse<AccountResponseDetail>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new AccountResponseDetail()
                };
            }
        }

        public async Task<DataResponse<AccountResponseOrderHisory>> GetAccountOrderHistory(string email)
        {
            try
            {
                var cachAccountOrderHistory = await cachService
                    .GetData<AccountResponseOrderHisory>($"account-{email}:orders");

                if (cachAccountOrderHistory.IsSuccess)
                {
                    return new DataResponse<AccountResponseOrderHisory>
                    {
                        Description = "",
                        StatusCode = StatusCode.Ok,
                        Data = cachAccountOrderHistory.Value
                    };
                }

                var account = await accountRepository.GetAccountWithOrderHistory(email);   
                
                if(account.IsFailure)
                {
                    return new DataResponse<AccountResponseOrderHisory>
                    {
                        Description = account.Error,
                        StatusCode = StatusCode.BadRequest,
                        Data = new AccountResponseOrderHisory()
                    };
                }

                var orderHistory = (await Task.WhenAll(
                    account.Value.ShopingHistory.Select(async x => new ProductResponse
                    {
                        Id = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        Description = x.Product.Description,
                        Count = x.Count,    
                        Images = await blobService.DownLoadBlobFolder(x.Product.ImageFolder)
                    })
                )).ToList();

                var accountOrderHistory = new AccountResponseOrderHisory()
                {
                    Name = account.Value.Name,
                    Email = account.Value.Email,
                    OrderHistory = orderHistory
                };

                await cachService.SetData($"account-{email}:orders", accountOrderHistory, 360);

                return new DataResponse<AccountResponseOrderHisory>
                {
                    Description = "Success get account order details",
                    StatusCode = StatusCode.Ok,
                    Data = accountOrderHistory
                };
            }
            catch
            {
                return new DataResponse<AccountResponseOrderHisory>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new AccountResponseOrderHisory()
                };
            }
        }
    }
}
