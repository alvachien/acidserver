// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace acidserver
{
    internal static class ISStandardScopes
    {
        public const string Address = "address";
        public const string AllClaims = "all_claims";
        public const string Email = "email";
        public const string OfflineAccess = "offline_access";
        public const string OpenId = "openid";
        public const string Phone = "phone";
        public const string Profile = "profile";
        public const string Roles = "roles";
    }

    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                new Scope
                {
                    Name = "api.hihapi",
                    DisplayName = "HIH API",
                    Description = "All HIH features and data",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },

                new Scope
                {
                    Name = "api.acgallery",
                    DisplayName = "API for gallery file part",
                    Description = "All Gallery features and data",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "AC HIH App",
                    ClientId = "achihui.js",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "http://localhost:29521/logincallback.html"
#else
                        "http://achihui.azurewebsites.net/logincallback.html"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Roles.Name,
                        StandardScopes.OfflineAccess.Name,
                        StandardScopes.Email.Name,
                        "api.hihapi"
                    }
                },
                new Client
                {
                    ClientName = "AC Photo Gallery",
                    ClientId = "acgallery.app",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "http://localhost:1601/logincallback.html"
#else
                        "http://acgallery.azurewebsites.net/logincallback.html"
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:1601/index.html"
#else
                        "http://acgallery.azurewebsites.net/index.html"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Roles.Name,
                        StandardScopes.OfflineAccess.Name,
                        StandardScopes.Email.Name,
                        "api.hihapi",
                        "api.acgallery"
                    }
                }
            };
        }
    }
}