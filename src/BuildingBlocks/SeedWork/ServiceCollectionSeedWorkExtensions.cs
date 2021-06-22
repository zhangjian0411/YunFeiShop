using System;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.EventBus.RabbitMQ;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Application.Behaviors;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency;

namespace ZhangJian.YunFeiShop.BuildingBlocks.SeedWork
{
    public static class ServiceCollectionSeedWorkExtensions
    {
        public static IServiceCollection AddSeedWork<T>(this IServiceCollection services, string clientName) where T : DbContextBase
        {
            return AddSeedWork<T>(services, clientName, "localhost");
        }

        public static IServiceCollection AddSeedWork<T>(this IServiceCollection services, string clientName, string mqHostName) where T : DbContextBase
        {
            if (string.IsNullOrWhiteSpace(clientName)) throw new ArgumentException("The parameter 'clientName' must have a value.");

            services.AddEventBusRabbitMQ(clientName, mqHostName);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            
            services.AddIntegrationEvent<T>();
            
            services.AddIdentifiedCommand<T>();
            
            return services;
        }

        private static IServiceCollection AddIdentifiedCommand<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddTransient<IRequestManager, RequestManager<T>>();

            return services;
        }
    }
}