using Application.Contracts.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.FeedBack;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Entities;
using Product.Services.Interfaces;

namespace Product.Services.Implementations
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepository feedBackRepository;
        private readonly IBlobService blobService;
        private readonly IAccountRepository accountRepository;
        private readonly IProductRepository productRepository;

        public FeedBackService(
            IFeedBackRepository feedBackRepository,
            IBlobService blobService,
            IAccountRepository accountRepository,
            IProductRepository productRepository)
        {
            this.feedBackRepository = feedBackRepository;
            this.blobService = blobService;
            this.accountRepository = accountRepository;
            this.productRepository = productRepository;
        }

        public async Task<DataResponse<FeedBackResponse>> GetFeedBackAccount(long productId, string emailAccount)
        {
            try
            {
                var product = await productRepository.GetProduct(productId);

                if(product.IsFailure)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Product Id",
                        Data = new()
                    };
                }

                var account = await accountRepository.GetAccountByEmail(emailAccount);
            
                if(account.IsFailure)
                {
                    return new DataResponse<FeedBackResponse> 
                    { 
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Email Account",
                        Data = new()
                    };
                }

                var feedBackAccount = await feedBackRepository.GetFeedBacksProduct(productId)
                    .FirstOrDefaultAsync(x => x.Owner.Email == emailAccount);

                if(feedBackAccount is null)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "FeedBack Not Exist From this Account",
                        Data = new()
                    };
                }

                return new DataResponse<FeedBackResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get User FeedBack",
                    Data = new FeedBackResponse 
                    { 
                        Id = feedBackAccount.Id,
                        FeedBackText = feedBackAccount.FeedBackText,
                        ProductId = feedBackAccount.Product.Id,
                        SendTime = feedBackAccount.SendTime,
                        Rate = feedBackAccount.Rate,
                        OwnerFeedBack = new ProductSeller 
                        { 
                            Email = feedBackAccount.Owner.Email,
                            Name = feedBackAccount.Owner.Name,
                        },
                        Images = await blobService.DownLoadBlobFolder(feedBackAccount.ImageFilder)
                    }

                };
            }
            catch
            {
                return new DataResponse<FeedBackResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<List<FeedBackResponse>>> GetFeedBacksProduct(long productId)
        {
            try
            {
                var product = await productRepository.GetProduct(productId);

                if(product.IsFailure)
                {
                    return new DataResponse<List<FeedBackResponse>>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Product is not exist",
                        Data = new()
                    };
                }

                var feedBacks = await feedBackRepository
                    .GetFeedBacksProduct(productId)
                    .ToListAsync();

                var feedBackWithImages = await Task.WhenAll(
                    feedBacks.Select(async x =>
                    {
                        return new FeedBackResponse
                        {
                            Id = x.Id,
                            FeedBackText = x.FeedBackText,
                            Rate = x.Rate,
                            SendTime = x.SendTime,
                            ProductId = x.Product.Id,
                            Images = await blobService.DownLoadBlobFolder(x.ImageFilder),
                            OwnerFeedBack = new ProductSeller
                            {
                                Email = x.Owner.Email,
                                Name = x.Owner.Name
                            }
                        };
                    }).ToList());

                return new DataResponse<List<FeedBackResponse>>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success Get Product FeedBack",
                    Data = feedBackWithImages.ToList()
                };
            }
            catch(Exception ex  )
            {
                return new DataResponse<List<FeedBackResponse>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error",
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<FeedBackPaginationResponse>> GetFeedBacksProductWithPagination(
            long productId, int start, int count, int? rating)
        {
            try
            {
                if(start < 1 || count < 0)
                {
                    return new DataResponse<FeedBackPaginationResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid sart or count",
                        Data = new()
                    };
                }

                var product = await productRepository.GetProduct(productId);
                
                if(product.IsFailure)
                {
                    return new DataResponse<FeedBackPaginationResponse>
                    {
                        Description = "Not Found Product with same id",
                        StatusCode = StatusCode.BadRequest,
                        Data = new()
                    };
                }

                var feedBacks = feedBackRepository.GetFeedBacksProduct(productId, rating);

                var feedBacksPagination = await feedBacks
                    .Skip((start - 1) * count)
                    .Take(count)
                    .ToListAsync();

                var feedBackWithImages = await Task.WhenAll(
                    feedBacksPagination.Select(async x =>
                    {
                        return new FeedBackResponse
                        {
                            Id = x.Id,
                            FeedBackText = x.FeedBackText,
                            Rate = x.Rate,
                            SendTime = x.SendTime,
                            ProductId = x.Product.Id,
                            Images = await blobService.DownLoadBlobFolder(x.ImageFilder),
                            OwnerFeedBack = new ProductSeller
                            {
                                Email = x.Owner.Email,
                                Name = x.Owner.Name
                            }
                        };
                    }).ToList());

                return new DataResponse<FeedBackPaginationResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get Product FeedBacks",
                    Data = new FeedBackPaginationResponse
                    {
                        Count = count,
                        Page = start,
                        MaxCount = await feedBacks.CountAsync(),
                        FeedBacks = feedBackWithImages.ToList()
                    }
                };
            }
            catch
            {
                return new DataResponse<FeedBackPaginationResponse>
                {
                    Description = "Internal Server Error",
                    StatusCode = StatusCode.InternalServerError,
                    Data = new ()
                };
            }
        }

        public async Task<DataResponse<FeedBackResponse>> UploadFeedBack(FeedBackAddRequest request)
        {
            try
            {
                var user = await accountRepository.GetAccountByEmail(request.EmailAccount);

                if(user.IsFailure)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid account that sended feedback",
                        Data = new()
                    };
                }

                var product = await productRepository.GetProduct(request.ProductId);

                if(product.IsFailure)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Product with sended productId is not exist",
                        Data= new()
                    };
                }

                var feedBackUser = await feedBackRepository
                    .GetFeedBacksProduct(request.ProductId)
                    .FirstOrDefaultAsync(x => user.Value.Email == x.Owner.Email);

                if(feedBackUser is not null)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "You have alreade sended feedback",
                        Data = new ()
                    };
                }

                var feedBack = FeedBack.Initialize(
                    request.Rate,
                    product.Value,
                    user.Value,
                    request.Text);


                if(feedBack.IsFailure)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Invalid Request - " + feedBack.Error,
                        Data = new()
                    };
                }

                var uploadedFeedBack = await feedBackRepository.AddFeedBack(feedBack.Value);

                if(uploadedFeedBack.IsFailure)
                {
                    return new DataResponse<FeedBackResponse>
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Internal Server Error(upload data)",
                        Data = new()
                    };
                }

                var feedBackUploadImages = new List<string>();
                if(request.FeedBackImages is not null)
                {
                    feedBackUploadImages =  await blobService.UploadBlob(
                        uploadedFeedBack.Value.ImageFilder,
                        request.FeedBackImages);
                }

                return new DataResponse<FeedBackResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Create New FeedBack",
                    Data = new FeedBackResponse
                    {
                        Id = uploadedFeedBack.Value.Id,
                        SendTime = uploadedFeedBack.Value.SendTime,
                        FeedBackText = uploadedFeedBack.Value.FeedBackText,
                        ProductId = product.Value.Id,
                        Rate = uploadedFeedBack.Value.Rate,
                        Images = feedBackUploadImages,
                        OwnerFeedBack = new ProductSeller
                        {
                            Email = user.Value.Email,
                            Name = user.Value.Name,
                        }
                    }
                };
            }
            catch
            {
                return new DataResponse<FeedBackResponse>
                {
                    Description = "Internal Server Error",
                    StatusCode = StatusCode.InternalServerError,
                    Data = new()
                };
            }
        }
    }
}
