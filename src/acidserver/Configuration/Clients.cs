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
                        "http://localhost:1601/logincallback.html"
#else
                        "http://acgallery.azurewebsites.net/logincallback.html"
#endif
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
#if DEBUG
                        "http://localhost:1601/logoutcallback.html"
#else
                        "http://acgallery.azurewebsites.net/logoutcallback.html"
#endif
                    },
                    AllowedScopes = new List<String>
                    {
                        IdentityServer4.Constants.StandardScopes.OpenId,
                        IdentityServer4.Constants.StandardScopes.Profile,
                        IdentityServer4.Constants.StandardScopes.Email,
                        IdentityServer4.Constants.StandardScopes.Roles,
                        IdentityServer4.Constants.StandardScopes.AllClaims,
                        "api.hihapi",
                        "api.acgallery"
                    }
                }
            };
        }
    }
}
