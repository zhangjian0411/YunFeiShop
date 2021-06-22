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
using Microsoft.OpenApi.Models;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork;
using ZhangJian.YunFeiShop.Services.Ordering.API.Application.Queries;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering", Version = "v1" });
            });

            services.AddDbContext<OrderingContext>(options =>
            {
                options.UseSqlite("Data Source=./data/ordering.db",
                    sqliteOptions => sqliteOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            });
            
            services.AddMediatR(typeof(Startup));
            services.AddSeedWork<OrderingContext>("Ordering", Configuration["Services:RabbitMQ"]);

            services.AddTransient(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient<IOrderQueries, OrderQueries>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering v1"));
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
