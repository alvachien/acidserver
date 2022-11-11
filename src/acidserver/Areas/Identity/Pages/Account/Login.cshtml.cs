using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Events;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public LoginModel(
            SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
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

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            #region Generated codes from ASP.NET Core Identity
            //returnUrl ??= Url.Content("~/");

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //if (ModelState.IsValid)
            //{
            //    // This doesn't count login failures towards account lockout
            //    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("User logged in.");
            //        return LocalRedirect(returnUrl);
            //    }
            //    if (result.RequiresTwoFactor)
            //    {
            //        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            //    }
            //    if (result.IsLockedOut)
            //    {
            //        _logger.LogWarning("User account locked out.");
            //        return RedirectToPage("./Lockout");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //        return Page();
            //    }
            //}

            //// If we got this far, something failed, redisplay form
            //return Page();
            #endregion

            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            //// the user clicked the "cancel" button
            //if (button != "login")
            //{
            //    if (context != null)
            //    {
            //        // if the user cancels, send a result back into IdentityServer as if they 
            //        // denied the consent (even if this client does not require consent).
            //        // this will send back an access denied OIDC error response to the client.
            //        await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            //        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            //        if (context.IsNativeClient())
            //        {
            //            // The client is native, so this change in how to
            //            // return the response is for better UX for the end user.
            //            return this.LoadingPage("Redirect", model.ReturnUrl);
            //        }

            //        return Redirect(model.ReturnUrl);
            //    }
            //    else
            //    {
            //        // since we don't have a valid context, then we just go back to the home page
            //        return Redirect("~/");
            //    }
            //}

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(Input.Email);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        //if (context.IsNativeClient())
                        //{
                        //    // The client is native, so this change in how to
                        //    // return the response is for better UX for the end user.
                        //    return this.LoadingPage("Redirect", returnUrl);
                        //}

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(returnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else if (string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(Input.Email, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // something went wrong, show form with error
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
