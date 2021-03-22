using System;
using System.Data.Common;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.EventBus;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.EventBus.RabbitMQ;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Services;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEventService<TDbContext>(this IServiceCollection services, string clientName) where TDbContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(clientName)) throw new ArgumentException("The parameter 'clientName' must have a value.");

            // Add services.
            services.AddRabbitMQEventBus(clientName);

            // services.AddDbContext<IntegrationEventLogContext>((svc, dbContextOptions) => {
            //     var connection = svc.GetRequiredService<TDbContext>().Database.GetDbConnection();
            //     options.EventLogContextOptionsAction(dbContextOptions, connection);
            // });

            services.AddTransient<Func<DbConnection, IIntegrationEventPersistenceService>>(sp => 
                connection =>
                {
                    var dbContext = new IntegrationEventEntryContext(
                        new DbContextOptionsBuilder<IntegrationEventEntryContext>()
                            .UseSqlite(connection)
                            .Options
                    );
                    return new DefaultIntegrationEventPersistenceService(dbContext);
                }
            );
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
            services.AddTransient<IIntegrationEventService, DefaultIntegrationEventService<TDbContext>>();

            services.AddAllEventHandlers();

            return services;
        }

        private static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services, string clientName)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = "localhost",
                        DispatchConsumersAsync = true
                    };

                    var retryCount = 5;

                    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
                });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    // var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, clientName, retryCount);
                });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

        private static IServiceCollection AddAllEventHandlers(this IServiceCollection services)
        {
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