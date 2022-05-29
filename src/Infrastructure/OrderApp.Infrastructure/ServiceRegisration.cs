using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApp.Application.Interfaces.IService;
using OrderApp.Infrastructure.Services;
using OrderApp.Infrastructure.Services.BackgroundService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure
{
    public static class ServiceRegisration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IMessageProducer, RabbitMQProducer>();
            services.AddSingleton<IHostedService, MailSenderBackgroundService>();
        }
    }
}
