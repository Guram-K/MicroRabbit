using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Data.Context
{
    public class BankingDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {

        }
    }
}
