using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repositories;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.core.Bus;
using MicroRabbit.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infrastructure.IOC
{
    public class DependencyContainer
    {

        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            services.AddTransient<IEventBus, RabbitmqBus>();

            //Application Services
            services.AddTransient<IAccountService, AccountService>();

            //Data
            services.AddTransient<IAccountRepository, AccountRepository>();
            //services.AddTransient<BankingDbContext>();
        }
    }
}
