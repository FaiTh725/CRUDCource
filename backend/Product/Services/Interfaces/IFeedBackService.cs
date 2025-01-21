using Application.Contracts.Response;
using Product.Domain.Contracts.Models.FeedBack;

namespace Product.Services.Interfaces
{
    public interface IFeedBackService
    {
        Task<DataResponse<FeedBackResponse>> UploadFeedBack(FeedBackAddRequest request);

        Task<DataResponse<List<FeedBackResponse>>> GetFeedBacksProduct(long productId);
    }
}
