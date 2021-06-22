// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity {
    public class Startup {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup (IWebHostEnvironment environment, IConfiguration configuration) {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices (IServiceCollection services) {
            services.AddControllersWithViews ();


            services.AddCustomDataProtection(Configuration);


            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

                // options.Authentication.CookieSameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                // options.Authentication.CheckSessionCookieSameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            });
            

            // in-memory, code config
            builder.AddInMemoryIdentityResources (Config.IdentityResources);
            builder.AddInMemoryApiScopes (Config.ApiScopes);
            builder.AddInMemoryClients (Config.Clients);
            builder.AddTestUsers (TestUsers.Users);

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential(filename: "tempkey.jwk");
            // var password = "123456";
            // var certificate = @"C:\Users\pc-amd\Source\cert\demoyunfeicom.pfx";
            // Debug.Assert(password == "123456", "JWT:Secret is correct!");
            // var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(
            //     certificate,
            //     password
            // );
            // builder.AddSigningCredential(cert);
        }

        public void Configure (IApplicationBuilder app) {
            if (Environment.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseIdentityServer ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapDefaultControllerRoute ();
            });
        }
    }

    public static class CustomServiceCollectionExtensions
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
    }
}