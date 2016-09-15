using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acidserver.Configuration
{
    public class Clients
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

        public static IEnumerable<Client> Get()
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
                        ISStandardScopes.OpenId,
                        ISStandardScopes.Profile,
                        ISStandardScopes.Email,
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
                        ISStandardScopes.OpenId,
                        ISStandardScopes.Profile,
                        ISStandardScopes.Email,
                        ISStandardScopes.Roles,
                        ISStandardScopes.AllClaims,
                        "api.hihapi",
                        "api.acgallery"
                    }
                }
            };
        }
    }
}
