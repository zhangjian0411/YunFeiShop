using System;
using System.Data.Common;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.EventBus.RabbitMQ;
using ZhangJian.YunFeiShop.BuildingBlocks.Integration.DefaultService;
using ZhangJian.YunFeiShop.BuildingBlocks.Integration.Exceptions;
using ZhangJian.YunFeiShop.BuildingBlocks.Integration.IntegrationEventLog;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration
{ 
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEventService<TDbContext>(this IServiceCollection services, Action<IntegrationEventServiceOptions> setupAction) where TDbContext : DbContext
        {
            // Build options.
            var options = new IntegrationEventServiceOptions();
            setupAction(options);

            if (options.EventLogContextOptionsAction == null)
            {
                throw new OptionRequiredException(nameof(options.EventLogContextOptionsAction));
            }

            // Add services.
            services.AddRabbitMQEventBus(options);

            services.AddDbContext<IntegrationEventLogContext>((svc, dbContextOptions) => {
                var connection = svc.GetRequiredService<TDbContext>().Database.GetDbConnection();
                options.EventLogContextOptionsAction(dbContextOptions, connection);
            });

            services.AddTransient<IntegrationEventLogService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
            services.AddTransient<IIntegrationEventService, IntegrationEventService<TDbContext>>();

            services.AddAllEventHandlers();

            return services;
        }

        private static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services, IntegrationEventServiceOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ClientName))
            {
                throw new OptionRequiredException(nameof(options.ClientName));
            }

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

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, options.ClientName, retryCount);
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

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            var genericInterfaceType = typeof(IIntegrationEventHandler<>);

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
                    
                    var eventType = serviceType.GetGenericArguments()[0];
                    eventBus.Subscribe(eventType, serviceType);
                });


            return app;
        }
    }
}