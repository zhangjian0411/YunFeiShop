using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents;

namespace ZhangJian.YunFeiShop.BuildingBlocks.SeedWork
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSeedWork(this IApplicationBuilder app)
        {
            app.UseIntegrationEvent();

            return app;
        }
    }
}