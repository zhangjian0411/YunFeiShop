using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services.Implements;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping
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

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "BFF.Spa.Shopping", Version = "v1" }));

            services.AddCustomDataProtection(Configuration)
                    .AddCustomAuthentication(Configuration)
                    .AddApiServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BFF.Spa.Shopping v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization();
            });
        }
    }

    public static class CustomStartupExtensionMethods
    {
        public static IServiceCollection AddCustomDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            var applicatioName = configuration.GetValue<string>("DataProtection:ApplicationName");
            var persistKeysDirectory = configuration.GetValue<string>("DataProtection:PersistKeysDirectory");

            var dataBuilder = services.AddDataProtection();

            if (!string.IsNullOrEmpty(persistKeysDirectory))
            {
                dataBuilder.PersistKeysToFileSystem(new DirectoryInfo(persistKeysDirectory));
            }

            if (!string.IsNullOrEmpty(applicatioName))
            {
                dataBuilder.SetApplicationName(applicatioName);
            }

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = configuration.GetValue<string>("Authentication:CookieName");
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetValue<string>("Authentication:OpenIdConnect:Authority");

                    options.ClientId = configuration.GetValue<string>("Authentication:OpenIdConnect:ClientId");
                    options.ClientSecret = configuration.GetValue<string>("Authentication:OpenIdConnect:ClientSecret");

                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.Scope.Add("offline_access");
                    options.Scope.Add("svc.catalog");

                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";

                    // options.SignedOutRedirectUri = "/catalog";
                });

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAccessTokenManagement();


            services.AddHttpClient<ICatalogService, CatalogHttpClient>(
                client => client.BaseAddress = new Uri(configuration.GetSection("Services:Catalog").Value)
            ).AddUserAccessTokenHandler();

            services.AddHttpClient<ICartService, CartHttpClient>(
                client => client.BaseAddress = new Uri(configuration.GetSection("Services:Cart").Value)
            ).AddUserAccessTokenHandler();

            services.AddHttpClient<IOrderService, OrderHttpClient>(
                client => client.BaseAddress = new Uri(configuration.GetSection("Services:Order").Value)
            ).AddUserAccessTokenHandler();

            return services;
        }
    }
}
