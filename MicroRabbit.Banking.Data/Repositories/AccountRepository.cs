using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _bankingDbContext;

        public AccountRepository(BankingDbContext bankingDbContext)
        {
            _bankingDbContext = bankingDbContext;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _bankingDbContext.Accounts;
        }
    }
}
