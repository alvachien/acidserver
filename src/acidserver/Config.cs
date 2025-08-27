using Duende.IdentityServer.Models;
using static Duende.IdentityServer.IdentityServerConstants;

namespace IdentityServerAspNetIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("api.hih", "HIH API"),
                new ApiScope("api.acgallery", "Gallery API"),
                new ApiScope("api.knowledgebuilder", "Knowledge Builder API")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientName = "AC HIH App",
                ClientId = "achihui.js",
                AllowedGrantTypes = GrantTypes.Code,

                RequireClientSecret = false,
                //RequirePkce = true,
                RequirePkce = false,

                AccessTokenLifetime = 3600, // 3600 seconds
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true, // For refresh token

                RequireConsent = false,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,

                RedirectUris = {
#if DEBUG
#if USE_ALIYUN
                    "https://www.alvachien.com/hih/signin-callback"
#else
                    "https://localhost:29528/signin-callback"
#endif
#endif
                },
                PostLogoutRedirectUris = {
#if DEBUG
#if USE_ALIYUN
                    "https://www.alvachien.com/hih"
#else
                    "https://localhost:29528"
#endif
#endif
                },
                AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Email,
                    StandardScopes.OfflineAccess,
                    "api.hih"
                },
                AlwaysIncludeUserClaimsInIdToken = true
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
                RefreshTokenUsage = TokenUsage.OneTimeOnly,

                RequireConsent = false,
                RedirectUris = {
#if USE_ALIYUN
                   "https://www.alvachien.com/gallery/signin-callback"
#else
                   "https://localhost:16001/signin-callback"
#endif
                },
                PostLogoutRedirectUris = {
#if USE_ALIYUN
                   "https://www.alvachien.com/gallery"
#else
                   "https://localhost:16001"
#endif
                },
                AllowedScopes = {
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
                RefreshTokenUsage=TokenUsage.OneTimeOnly,

                RequireConsent = false,
                RedirectUris = {
#if USE_ALIYUN
                    "https://www.alvachien.com/learning/signin-callback"
#else
                    "https://localhost:44367/signin-callback",
                    "http://localhost:44367/signin-callback"
#endif
                },
                PostLogoutRedirectUris = {
#if USE_ALIYUN
                    "https://www.alvachien.com/learning"
#else
                    "https://localhost:44367",
                    "http://localhost:44367"
#endif
                },
                
                AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Email,
                    StandardScopes.OfflineAccess,
                    "api.knowledgebuilder"
                },
                AccessTokenLifetime = 3600
            }
        };
}
