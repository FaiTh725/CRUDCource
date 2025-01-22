using Application.Contracts.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Models;
using Product.Services.Interfaces;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IBlobService blobService;
        private readonly ITelemetryService telemetryService;
        private readonly IFeedBackRepository feedBackRepository;

        public ProductService(
            IProductRepository productRepository,
            IBlobService blobService,
            IAccountRepository accountRepository,
            ITelemetryService telemetryService,
            IFeedBackRepository feedBackRepository)
        {
            this.blobService = blobService;
            this.productRepository = productRepository;
            this.accountRepository = accountRepository;
            this.telemetryService = telemetryService;
            this.feedBackRepository = feedBackRepository;
        }

        public async Task<DataResponse<ProductResponse>> GetProduct(long productId)
        {
            try
            {
                var product = await productRepository.GetProduct(productId);


                if(product.IsFailure)
                {
                    return new DataResponse<ProductResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Id Product",
                        Data = new ProductResponse()
                    };
                }

                var productImages = await blobService.DownLoadBlobFolder(product.Value.ImageFolder);

                return new DataResponse<ProductResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success get product",
                    Data = new ProductResponse
                    {
                        Id = product.Value.Id,
                        Description = product.Value.Description, 
                        Name = product.Value.Name,
                        Price = product.Value.Price,
                        Images = productImages,
                        Count = product.Value.Count
                    }
                };
            }
            catch
            {
                return new DataResponse<ProductResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new ProductResponse()
                };
            }
        }

        public async Task<DataResponse<ProductMetricsResponse>> GetProductMetrics(long productId)
        {
            try
            {
                var product = await productRepository.GetProduct(productId);

                if (product.IsFailure)
                {
                    return new DataResponse<ProductMetricsResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Product Id",
                        Data = new()
                    };
                }

                var generalRating = await telemetryService.GetProductGeneralRating(productId);
                if(generalRating.IsFailure)
                {
                    return new DataResponse<ProductMetricsResponse>
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = generalRating.Error,
                        Data = new()
                    };
                }

                var extentionRating = await telemetryService.GetProductExtentionRating(productId);

                if(extentionRating.IsFailure)
                {
                    return new DataResponse<ProductMetricsResponse>
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = extentionRating.Error,
                        Data = new()
                    };
                }

                var countFeedBacks = feedBackRepository
                    .GetFeedBacksProduct(productId)
                    .Count();

                return new DataResponse<ProductMetricsResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get Product Rating",
                    Data = new ProductMetricsResponse
                    {
                        ProductId = product.Value.Id,
                        GeneralRating = Math.Round(generalRating.Value, 2),
                        PartRatingCount = extentionRating.Value,
                        MaxCount = countFeedBacks
                    }
                };
            }
            catch
            {
                return new DataResponse<ProductMetricsResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<ProductPaginationResponse>> GetProductPagination(int page, int count)
        {
            try
            {
                var products = productRepository.GetProducts();

                var productsPagination = await products
                    .Skip((page - 1)  * count)
                    .Take(count)
                    .ToListAsync();

                var productsWithImages = await Task.WhenAll(
                    productsPagination.Select(async x => new ProductResponse
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                        Price = x.Price,
                        Images = await blobService.DownLoadBlobFolder(x.ImageFolder),
                        Count = x.Count
                    }));

                return new DataResponse<ProductPaginationResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success Get products",
                    Data = new ProductPaginationResponse
                    {
                        Count = count,
                        Page = page,
                        Products = productsWithImages.ToList(),
                        MaxCount = await products.CountAsync()
                    }
                };
            }
            catch
            {
                return new DataResponse<ProductPaginationResponse> 
                { 
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new ()
                };

            }
        }

        public async Task<DataResponse<List<ProductResponse>>> GetProducts()
        {
            try
            {
                var products = productRepository.GetProducts();

                var productsPagination = await products.ToListAsync();

                var productsResponse = new List<ProductResponse>();

                foreach (var product in productsPagination)
                {
                    var productImages = await blobService.DownLoadBlobFolder(product.ImageFolder);

                    productsResponse.Add(new ProductResponse
                    {
                        Id = product.Id,
                        Description = product.Description,
                        Name = product.Name,
                        Price = product.Price,
                        Images = productImages,
                        Count = product.Count
                    });
                }

                return new DataResponse<List<ProductResponse>>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success Get products",
                    Data = productsResponse
                };
            }
            catch
            {
                return new DataResponse<List<ProductResponse>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new List<ProductResponse>()
                };

            }
        }

        public async Task<DataResponse<ProductSeller>> GetProductSeller(long productId)
        {
            try
            {
                var product = await productRepository.GetProductWithSeller(productId);

                if(product.IsFailure)
                {
                    return new DataResponse<ProductSeller>
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = product.Error,
                        Data = new ProductSeller()
                    };
                }

                return new DataResponse<ProductSeller>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get Seller Info",
                    Data = new ProductSeller
                    {
                        Email = product.Value.Sealer.Email,
                        Name = product.Value.Sealer.Name
                    }
                };
            }
            catch
            {
                return new DataResponse<ProductSeller>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new ProductSeller()
                };
            }
        }

        public async Task<BaseResponse> IsProductInCart(long productId, string email)
        {
            try
            {
                var account = await accountRepository.GetAccountWithCart(email);

                if(account.IsFailure)
                {
                    return new BaseResponse
                    {
                        Description = account.Error, 
                        StatusCode = StatusCode.BadRequest
                    };
                }

                var product = account.Value.ShopingCart
                    .FirstOrDefault(x => x.Product.Id == productId);

                if(product is null)
                {
                    return new BaseResponse
                    {
                        Description = "Product is not in account cart",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse
                {
                    Description = "Product in cart",
                    StatusCode = StatusCode.Ok
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

        public async Task<DataResponse<ProductResponse>> UploadProduct(UploadProduct product)
        {
            try
            {
                var sealer = await accountRepository.GetAccountByEmail(product.SealerEmail);

                if (sealer.IsFailure)
                {
                    return new DataResponse<ProductResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Email Sealer",
                        Data = new ProductResponse()
                    };
                }

                var newProductInstance = ProductEntity.Initialize(
                        product.Name, 
                        sealer.Value, 
                        product.Price,
                        product.Count);

                if(newProductInstance.IsFailure)
                {
                    return new DataResponse<ProductResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = newProductInstance.Error,
                        Data= new ProductResponse()
                    };
                }

                var newProduct = await productRepository.AddProduct(newProductInstance.Value);

                if(newProduct.IsFailure)
                {
                    return new DataResponse<ProductResponse>
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Unknown server error",
                        Data = new ProductResponse()
                    };
                }

                var productImagesUrl = new List<string>();
                if(product.Files != null)
                {
                    productImagesUrl = await blobService.UploadBlob(newProduct.Value.ImageFolder, product.Files);
                }

                return new DataResponse<ProductResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Succesfully upload product",
                    Data = new ProductResponse()
                    {
                        Id = newProduct.Value.Id,
                        Name = newProduct.Value.Name,
                        Description = newProduct.Value.Description,
                        Price = newProduct.Value.Price,
                        Images = productImagesUrl,
                        Count = newProduct.Value.Count
                    }
                };
            }
            catch
            {
                return new DataResponse<ProductResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new ProductResponse()
                };
            }
        }
    }
}
