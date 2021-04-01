using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.core.Bus;
using System.Collections.Generic;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IEventBus _bus;

        public AccountService(IAccountRepository accountRepo, IEventBus bus)
        {
            _accountRepo = accountRepo;
            _bus = bus;
        }

        public IEnumerable<Account> getAccounts()
        {
            return _accountRepo.GetAccounts();
        }

        public void Transfer(AccountTransfer accountTransfer)
        {
            var createTransferCommand = new CreateTransferCommand(accountTransfer.FromAccount, accountTransfer.ToAccount, accountTransfer.TransferAmount);
            _bus.SendCommand(createTransferCommand);
        }
    }
}
