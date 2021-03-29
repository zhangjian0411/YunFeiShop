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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork;
using ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.Services;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;
using ZhangJian.YunFeiShop.Services.Carts.Infrastructure;

namespace ZhangJian.YunFeiShop.Services.Carts.API
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
            services.AddControllers();services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cart", Version = "v1" });
            });

            services.AddDbContext<CartContext>(options =>
            {
                options.UseSqlite("Data Source=carts.db",
                    sqliteOptions => sqliteOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            });

            services.AddMediatR(typeof(Startup));

            services.AddSeedWork<CartContext>("Carts");

            services.AddAutoMapper(typeof(Startup));


            services.AddTransient(typeof(ICartRepository), typeof(CartRepository));
            services.AddTransient<IIdentityService, FakeIdentityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart v1"));
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
