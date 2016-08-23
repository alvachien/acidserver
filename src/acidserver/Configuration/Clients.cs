using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acidserver.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "achihui.js",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
                        "http://achihui.azurewebsites.net/logincallback.html"
                    },
                    AllowedScopes = new List<String>
                    {
                        IdentityServer4.Constants.StandardScopes.OpenId,
                        IdentityServer4.Constants.StandardScopes.Profile,
                        IdentityServer4.Constants.StandardScopes.Email,
                        "api.hihapi"
                    }
                },
                new Client
                {
                    ClientId = "acgallery.app",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "http://localhost:1601/callback.html"
#else
                        "http://acgallery.azurewebsites.net/callback.html"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        IdentityServer4.Constants.StandardScopes.OpenId,
                        IdentityServer4.Constants.StandardScopes.Profile,
                        IdentityServer4.Constants.StandardScopes.Email,
                        "api.hihapi"
                    }
                }
            };
        }
    }
}
