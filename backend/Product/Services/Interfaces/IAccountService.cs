using Application.Contracts.Response;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Request;

namespace Product.Services.Interfaces
{
    public interface IAccountService
    {
        Task<DataResponse<AccountResponseDetail>> GetAccountDetails(string email);

        Task<DataResponse<AccountResponseOrderHisory>> GetAccountOrderHistory(string email);

        Task<DataResponse<AccountResponseCartItems>> GetAccountCartItems(string email);

        Task<BaseResponse> CreateAccount(CreateAccount account);

        Task<BaseResponse> CreateRequestChangeRole(ChangeRoleAccount changeReuqest);

        Task<BaseResponse> CommitRequest(CommitRequestChangeRole request);

        Task<BaseResponse> AddProductToCart(AccountWithProductCountRequest request);

        Task<BaseResponse> DeleteProductFromCart(AccountWithProductRequest request);

        Task<BaseResponse> BuyProducts(AccountBuyProductRequest request);
    }
}
