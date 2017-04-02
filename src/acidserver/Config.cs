// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#define USE_MICROSOFTAZURE
//#define USE_ALIYUN

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using static IdentityServer4.IdentityServerConstants;

namespace acidserver
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()                
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(
                    "api.hihapi", 
                    "HIH API"
                    ),
                new ApiResource(
                    "api.galleryapi",
                    "Gallery API"
                    ),
                new ApiResource(
                    "api.acgallery",
                    "Gallery App"
                    )
            };
        }

        //// scopes define the resources in your system
        //public static IEnumerable<Scope> GetScopes()
        //{
        //    return new List<Scope>
        //    {
        //        StandardScopes.OpenId,
        //        StandardScopes.Profile,
        //        new Scope
        //        {
        //            Name = "api.hihapi",
        //            DisplayName = "HIH API",
        //            Description = "All HIH features and data",
        //            Type = ScopeType.Resource,
        //            Claims = new List<ScopeClaim>
        //            {
        //                new ScopeClaim("role")
        //            }
        //        },
        //        new Scope
        //        {
        //            Name = "api.galleryapi",
        //            DisplayName = "Gallery API",
        //            Description = "All Gallery features and data",
        //            Type = ScopeType.Resource,
        //            Claims = new List<ScopeClaim>
        //            {
        //                new ScopeClaim("role")
        //            }
        //        },
        //        new Scope
        //        {
        //            Name = "api.acgallery",
        //            DisplayName = "API for gallery file part",
        //            Description = "All Gallery features and data",
        //            Type = ScopeType.Resource,
        //            Claims = new List<ScopeClaim>
        //            {
        //                new ScopeClaim("role")
        //            }
        //        },
        //    };
        //}

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
                        "http://localhost:29521/logincallback.html",
                        "https://localhost:29521/logincallback.html"
#else
#if USE_MICROSOFTAZURE
                        "http://achihui.azurewebsites.net/logincallback.html",
                        "https://achihui.azurewebsites.net/logincallback.html"
#elif USE_ALIYUN
                        "https://118.178.58.187:5200/loginccallback.html",
                        "http://118.178.58.187:5200/logincallback.html"
#endif
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:29521/",
                        "https://localhost:29521/"
#else
#if USE_MICROSOFTAZURE
                        "http://achihui.azurewebsites.net/",
                        "https://achihui.azurewebsites.net/"
#elif USE_ALIYUN
                        "https://118.178.58.187:5200/",
                        "http://118.178.58.187:5200/"
#endif
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.OfflineAccess,
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
                        "https://localhost:1601/logincallback.html",
                        "http://localhost:1601/logincallback.html"
#else
#if USE_MICROSOFTAZURE
                        "http://acgallery.azurewebsites.net/logincallback.html",
                        "https://acgallery.azurewebsites.net/logincallback.html"
#elif USE_ALIYUN
                        "http://118.178.58.187:5210/logincallback.html",
                        "https://118.178.58.187:5210/logincallback.html"
#endif
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:1601/index.html",
                        "http://localhost:1601/index.html"
#else
#if USE_MICROSOFTAZURE
                        "http://acgallery.azurewebsites.net/index.html",
                        "https://acgallery.azurewebsites.net/index.html"
#elif USE_ALIYUN
                        "http://118.178.58.187:5210/index.html",
                        "https://118.178.58.187:5210/index.html"
#endif
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.OfflineAccess,
                        StandardScopes.Email,
                        "api.galleryapi",
                        "api.acgallery"
                    }
                }
            };
        }
    }
}
