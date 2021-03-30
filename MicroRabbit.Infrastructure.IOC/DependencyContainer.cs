using MicroRabbit.Domain.core.Bus;
using MicroRabbit.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infrastructure.IOC
{
    public class DependencyContainer
    {

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IEventBus, RabbitmqBus>();
        }
    }
}
