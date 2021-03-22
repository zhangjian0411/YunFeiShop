using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Abstractions;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.EventBus;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIntegrationEventService(this IApplicationBuilder app)
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