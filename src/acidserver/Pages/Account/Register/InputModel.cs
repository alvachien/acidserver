using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Pages.Register;

public class InputModel
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
}
