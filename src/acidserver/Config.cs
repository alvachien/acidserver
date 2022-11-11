using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using static Duende.IdentityServer.IdentityServerConstants;

namespace WebApplication1
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
                 new ApiScope("api.acgallery", "Gallery API"),
                 new ApiScope("api.knowledgebuilder", "Knowledge Builder API")
            };

#if DEBUG
        //public static List<TestUser> TestUsers =>
        //    new()
        //    {
        //        new TestUser()
        //        {
        //            SubjectId = "C731D080-1B8A-4152-833A-431FCD099C01",
        //            Username = "Admin",
        //            Password = "password"
        //        }
        //    };
#endif

        public static IEnumerable<Client> Clients => new List<Client>
            {
                new Client
                {
                    ClientName = "AC HIH App",
                    ClientId = "achihui.js",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true, // For refresh token

                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    RedirectUris = new List<String>
                    {
#if DEBUG
#if USE_ALIYUN
                        "https://www.alvachien.com/hih"
#else
                        "http://localhost:29521",
                        "https://localhost:29521"
#endif
#else
                        "https://www.alvachien.com"
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
#if USE_ALIYUN
                        "https://www.alvachien.com/hih"
#else
                        "http://localhost:29521",
                        "https://localhost:29521"
#endif
#else
                        "https://www.alvachien.com"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.OfflineAccess,
                        "api.hih"
                    },
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    ClientName = "AC Photo Gallery",
                    ClientId = "acgallery.app",
                    //AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true, // For refresh token

                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
#if USE_ALIYUN
                        "https://www.alvachien.com/gallery"
#else
                        "https://localhost:16001"
#endif
#else
                        "https://www.alvachien.com"
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
#if USE_ALIYUN
                        "https://www.alvachien.com/gallery"
#else
                        "https://localhost:16001"
                        // "http://localhost:16001"
#endif
#else
                        "https://www.alvachien.com"
#endif
                    },
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.OfflineAccess,
                        StandardScopes.Email,
                        "api.acgallery"
                    },
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    ClientName = "Knowledge Builder",
                    ClientId = "knowledgebuilder.js",
                    //AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true, // For refresh token
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
#if USE_ALIYUN
                        "https://www.alvachien.com/math"
#else
                        "http://localhost:44367",
                        "https://localhost:44367"
#endif
#else
                        "https://www.alvachien.com"
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
#if USE_ALIYUN
                        "https://www.alvachien.com/math"
#else
                        "http://localhost:44367",
                        "https://localhost:44367"
#endif
#else
                        "https://www.alvachien.com"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.OfflineAccess,
                        "api.knowledgebuilder"
                   },
                    AccessTokenLifetime = 3600
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
                        "api.hih",
                        "api.knowledge"
                    }
#endif
                }
        };        
    }
}
