using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using static Duende.IdentityServer.IdentityServerConstants;

namespace acidserver
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                 new ApiScope("api.hih", "HIH API"),
                 new ApiScope("api.acgallery", "Gallery App"),
                 new ApiScope("api.acquiz", "Quiz App")
            };

#if DEBUG
        public static List<TestUser> TestUsers =>
            new()
            {
                new TestUser()
                {
                    SubjectId = "C731D080-1B8A-4152-833A-431FCD099C01",
                    Username = "Admin",
                    Password = "password"
                }
            };
#endif

        public static IEnumerable<Client> Clients => new List<Client>
            {
                new Client
                {
                    ClientName = "AC HIH App",
                    ClientId = "achihui.js",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true, // For refresh toekn
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    RequireClientSecret = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "http://localhost:29521/logincallback.html",
                        "https://localhost:29521/logincallback.html"
#else
#if USE_AZURE
                        "https://achihui.azurewebsites.net/logincallback.html"
#elif USE_ALIYUN
                        "https://www.alvachien.com/hih/logincallback.html"
#endif
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:29521",
                        "https://localhost:29521"
#else
#if USE_AZURE
                        "https://achihui.azurewebsites.net"
#elif USE_ALIYUN
                        "https://www.alvachien.com/hih"
#endif
#endif
                    },
//                    AllowedCorsOrigins =
//                    {
//#if DEBUG
//                        "http://localhost:29521",
//                        "https://localhost:29521",
//                        "http://localhost:25688",
//                        "https://localhost:25688"
//#else
//#if USE_AZURE
//                        "https://achihui.azurewebsites.net"
//#elif USE_ALIYUN
//                        "https://www.alvachien.com/hih"
//#endif
//#endif
//                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.OfflineAccess,
                        "api.hih"
                    }
                },
                new Client
                {
                    ClientName = "AC Photo Gallery",
                    ClientId = "acgallery.app",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true, // For refresh toekn
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "https://localhost:16001/logincallback.html"
                        // "http://localhost:16001/logincallback.html"
#else
#if USE_AZURE
                        "https://acgallery.azurewebsites.net/logincallback.html"
#elif USE_ALIYUN
                        "https://www.alvachien.com/gallery/logincallback.html"
#endif
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "https://localhost:16001"
                        // "http://localhost:16001"
#else
#if USE_AZURE
                        "https://acgallery.azurewebsites.net"
#elif USE_ALIYUN
                        "https://www.alvachien.com/gallery"
#endif
#endif
                    },
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.OfflineAccess,
                        StandardScopes.Email,
                        "api.acgallery"
                    }
                },
                new Client
                {
                    ClientName = "AC Math Exercise",
                    ClientId = "acexercise.math",
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true, // For refresh token
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "http://localhost:20000/logincallback.html",
                        "https://localhost:20000/logincallback.html"
#else
#if USE_AZURE
                        "https://acmathexercise.azurewebsites.net/logincallback.html"
#elif USE_ALIYUN
                        "https://www.alvachien.com/math/logincallback.html"
#endif
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:20000",
                        "https://localhost:20000"
#else
#if USE_AZURE
                        "https://acmathexercise.azurewebsites.net"
#elif USE_ALIYUN
                        "https://www.alvachien.com/math"
#endif
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.OfflineAccess,
                        "api.acquiz"
                    }
#if DEBUG
                },
                new Client
                {
                    ClientId = "Postman",
                    ClientName = "Postman client",
                    // AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    // RedirectUris = { "https://oauth.pstmn.io/v1/callback" },
                    PostLogoutRedirectUris = { "https://www.getpostman.com" },
                    AllowedCorsOrigins = { "https://www.getpostman.com" },
                    EnableLocalLogin = true,

                    AllowedGrantTypes = new []
                    {
                        GrantType.ResourceOwnerPassword
                    },
                    ClientSecrets = { new Secret("Postman".Sha256()) },
                    ClientUri = null,
                    AllowOfflineAccess = true,
                    Enabled = true,
                    AllowedScopes = new []
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        // StandardScopes.OfflineAccess,
                        "api.acquiz",
                        "api.acgallery",
                        "api.hih"
                    }
#endif
                }

        };        
    }
}
