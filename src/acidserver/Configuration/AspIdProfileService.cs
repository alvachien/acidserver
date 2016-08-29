using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using acidserver.Models;

namespace acidserver.Configuration
{
    public class AspIdProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public AspIdProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.FindFirst("sub")?.Value;
            if (sub != null)
            {
                var user = await _userManager.FindByIdAsync(sub);

                // Roles
                var rls = await _userManager.GetRolesAsync(user);

                // Claims
                var cp = await _claimsFactory.CreateAsync(user);

                var claims = cp.Claims;
                if (context.AllClaimsRequested == false || 
                    (context.RequestedClaimTypes != null && context.RequestedClaimTypes.Any()))
                {
                    claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToArray().AsEnumerable();
                }

                List<System.Security.Claims.Claim> listClaims = new List<System.Security.Claims.Claim>();
                foreach (var cp2ld in claims)
                    listClaims.Add(cp2ld);

                var cp2 = await _userManager.GetClaimsAsync(user);
                if (cp2 != null && cp2.Count > 0)
                {
                    foreach (var cp2ld in cp2)
                        listClaims.Add(cp2ld);
                }

                context.IssuedClaims = listClaims;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var locked = true;

            var sub = context.Subject.FindFirst("sub")?.Value;
            if (sub != null)
            {
                var user = await _userManager.FindByIdAsync(sub);
                if (user != null)
                {
                    locked = await _userManager.IsLockedOutAsync(user);
                }
            }

            context.IsActive = !locked;
        }
    }
}
