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
                    RedirectUris = new List<string>
                    {
                        "http://achihui.azurewebsites.net/logincallback.html"
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid", "profile",
                        "api.hihapi"
                    }
                }
            };
        }
    }
}
