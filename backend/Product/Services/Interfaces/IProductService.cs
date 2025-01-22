using Application.Contracts.Response;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Product;

namespace Product.Services.Interfaces
{
    public interface IProductService
    {
        Task<DataResponse<ProductResponse>> UploadProduct(UploadProduct product);

        Task<DataResponse<ProductResponse>> GetProduct(long productId);

        Task<BaseResponse> IsProductInCart(long productId, string email);

        Task<DataResponse<ProductPaginationResponse>> GetProductPagination(int page, int count);

        Task<DataResponse<List<ProductResponse>>> GetProducts();

        Task<DataResponse<ProductSeller>> GetProductSeller(long productId);

        Task<DataResponse<ProductMetricsResponse>> GetProductMetrics(long productId);
    }
}
