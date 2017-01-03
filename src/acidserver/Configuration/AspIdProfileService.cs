using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using acidserver.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace acidserver.Configuration
{
    public class AspIdProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AspIdProfileService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.FindFirst("sub")?.Value;
            if (sub != null)
            {
                var user = await _userManager.FindByIdAsync(sub);
                List<System.Security.Claims.Claim> listClaims = new List<System.Security.Claims.Claim>();

                // Roles
                var rls = await _userManager.GetRolesAsync(user);
                foreach (var roleName in rls)
                {
                    var roleObj = await _roleManager.FindByNameAsync(roleName);
                    //var roleObj = await _roleManager.FindByIdAsync(roleName);

                    if (roleObj != null)
                    {
                        var cp0 = await _roleManager.GetClaimsAsync(roleObj);
                        foreach (var cp0cld in cp0)
                        {
                            listClaims.Add(cp0cld);
                        }
                    }
                }

                // Claims
                var cp = await _claimsFactory.CreateAsync(user);

                var claims = cp.Claims;
                if (context.RequestedClaimTypes != null && context.RequestedClaimTypes.Any())
                {
                    claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToArray().AsEnumerable();
                }

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