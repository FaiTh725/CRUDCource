using MassTransit;
using Product.Domain.Contracts.Models.Account;
using Product.Services.Interfaces;

namespace Product.Features.Consumers
{
    public class AccountCreateConsumer : IConsumer<CreateAccount>
    {
        private readonly IAccountService accountService;

        public AccountCreateConsumer(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task Consume(ConsumeContext<CreateAccount> context)
        {
            await accountService.CreateAccount(context.Message);
        }
    }
}
