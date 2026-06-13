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

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("api.hih", "HIH API")
            {
                UserClaims = { "name" }
            },
            new ApiResource("api.acgallery", "Gallery API")
            {
                UserClaims = { "name" }
            },
            new ApiResource("api.knowledgebuilder", "Knowledge Builder API")
            {
                UserClaims = { "name" }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api.hih", "HIH API"),
            new ApiScope("api.acgallery", "Gallery API"),
            new ApiScope("api.knowledgebuilder", "Knowledge Builder API")
        };

    public static IEnumerable<Client> GetClients(IReadOnlyList<ClientConfig> configClients)
    {
        var clientMap = configClients.ToDictionary(c => c.ClientId);

        var achihui = clientMap["achihui.js"];
        var acgallery = clientMap["acgallery.app"];
        var kb = clientMap["knowledgebuilder.js"];

        return new Client[]
        {
            new Client
            {
                ClientName = "AC HIH App",
                ClientId = achihui.ClientId,
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                AccessTokenLifetime = 3600, // 3600 seconds
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true, // For refresh token
                RequireConsent = false,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RedirectUris = achihui.RedirectUris.ToList(),
                PostLogoutRedirectUris = achihui.PostLogoutRedirectUris.ToList(),
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
                ClientId = acgallery.ClientId,
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true, // For refresh token
                AlwaysIncludeUserClaimsInIdToken = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RequireConsent = false,
                RedirectUris = acgallery.RedirectUris.ToList(),
                PostLogoutRedirectUris = acgallery.PostLogoutRedirectUris.ToList(),
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
                ClientId = kb.ClientId,
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true, // For refresh token
                AlwaysIncludeUserClaimsInIdToken = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RequireConsent = false,
                RedirectUris = kb.RedirectUris.ToList(),
                PostLogoutRedirectUris = kb.PostLogoutRedirectUris.ToList(),
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
}
