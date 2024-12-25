using Application.Contracts.Response;
using Product.Domain.Contracts.Models.Product;

namespace Product.Services.Interfaces
{
    public interface IProductService
    {
        Task<DataResponse<ProductResponse>> UploadProduct(UploadProduct product);

        Task<DataResponse<ProductResponse>> GetProduct(long productId);

        Task<BaseResponse> IsProductInCart(long productId, string email);

        Task<DataResponse<List<ProductResponse>>> GetProductPagination(int page, int count);

        Task<DataResponse<List<ProductResponse>>> GetProducts();
    }
}
