using System;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Services;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEvent<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
            services.AddTransient<IIntegrationEventPersistenceService, DefaultIntegrationEventPersistenceService<TDbContext>>();
            services.AddTransient<IIntegrationEventService, DefaultIntegrationEventService<TDbContext>>();

            services.AddClosedTypeOf(typeof(IIntegrationEventHandler<>));

            return services;
        }

        private static IServiceCollection AddClosedTypeOf(this IServiceCollection services, Type genericInterfaceType)
        {
            System.Reflection.Assembly.GetEntryAssembly().GetTypes()
                .Where(item => !item.IsAbstract &&
                                !item.IsInterface &&
                                item.GetInterfaces()
                                    .Where(i => i.IsGenericType)
                                    .Any(i => i.GetGenericTypeDefinition() == genericInterfaceType)
                )
                .ToList()
                .ForEach(assignedType =>
                {
                    var serviceType = assignedType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == genericInterfaceType);
                    services.AddTransient(serviceType, assignedType);
                });
            
            return services;
        }
    }
}