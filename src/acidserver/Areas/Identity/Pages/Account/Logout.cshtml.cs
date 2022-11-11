using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Events;
using Microsoft.AspNetCore.Authentication;
using Duende.IdentityServer.Extensions;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            #region Generated codes by Visual Studio
            //await _signInManager.SignOutAsync();
            //_logger.LogInformation("User logged out.");
            //if (returnUrl != null)
            //{
            //    return LocalRedirect(returnUrl);
            //}
            //else
            //{
            //    return RedirectToPage();
            //}
            #endregion

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            //if (vm.TriggerExternalSignout)
            //{
            //    // build a return URL so the upstream provider will redirect back
            //    // to us after the user has logged out. this allows us to then
            //    // complete our single sign-out processing.
            //    string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            //    // this triggers a redirect to the external provider for sign-out
            //    return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            //}

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
