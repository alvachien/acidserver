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
                    ClientName = "AC HIH App",
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
                    ClientName = "AC Photo Gallery",
                    ClientId = "acgallery.app",
                    AllowedGrantTypes = GrantTypes.Implicit,                    
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new List<String>
                    {
#if DEBUG
                        "http://localhost:1601"
#else
                        "http://acgallery.azurewebsites.net"
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:1601"
#else
                        "http://acgallery.azurewebsites.net"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        IdentityServer4.Constants.StandardScopes.OpenId,
                        IdentityServer4.Constants.StandardScopes.Profile,
                        IdentityServer4.Constants.StandardScopes.Email,
                        IdentityServer4.Constants.StandardScopes.Roles,
                        "api.hihapi"
                    }
                }
            };
        }
    }
}
