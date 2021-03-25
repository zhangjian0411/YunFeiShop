using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using ZhangJian.YunFeiShop.Services.Ordering.Infrastructure;

namespace ZhangJian.YunFeiShop.Services.Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<OrderingContext>(options =>
            {
                options.UseSqlite("Data Source=ordering.db",
                    sqliteOptions => sqliteOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            });
            // services.AddDbContext<BuildingBlocks.IntegrationEvents.Persistence.IntegrationEventEntryContext>(options =>
            // {
            //     options.UseSqlite("Data Source=ordering.db",
            //         sqliteOptions => sqliteOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            // });
            
            services.AddMediatR(typeof(Startup));
            services.AddSeedWork<OrderingContext>("Ordering");

            services.AddTransient(typeof(IOrderRepository), typeof(OrderRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSeedWork();
        }
    }
}
