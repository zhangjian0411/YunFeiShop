// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("svc.catalog")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "svc.catalog" }
                },

                new Client
                {
                    ClientId = "bff.spa.shopping",
                    ClientName = "BFF for SPA Shopping",
                    ClientSecrets = { new Secret("secret1101".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    UpdateAccessTokenClaimsOnRefresh = true,

                    // RedirectUris = { "https://localhost:6151/signin-oidc" },
                    // FrontChannelLogoutUri = "https://localhost:6151/signout-oidc",
                    // PostLogoutRedirectUris = { "https://localhost:6151/signout-callback-oidc" },
                    RedirectUris = { "https://demo.yunfei.com/signin-oidc" },
                    FrontChannelLogoutUri = "https://demo.yunfei.com/signout-oidc",
                    PostLogoutRedirectUris = { "https://demo.yunfei.com/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "svc.catalog"
                    }
                },

                new Client
                {
                    ClientId = "bff",
                    ClientName = "BFF",
                    ClientSecrets = { new Secret("secret1101".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    UpdateAccessTokenClaimsOnRefresh = true,

                    RedirectUris = { "https://localhost:7001/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7001/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "svc.catalog"
                    }
                }
            };
    }
}