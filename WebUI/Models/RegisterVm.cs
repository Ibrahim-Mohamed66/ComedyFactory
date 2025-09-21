using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels;

public class RegisterVm
{
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "This {0} field is required")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "This {0} field is required")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "This {0} field is required")]
    [Display(Name = "Mobile")]
    [RegularExpression(@"^(\+?\d{9,17})$", ErrorMessage = "Invalid Mobile Number")]
    public string Mobile { get; set; }

    public Gender? Gender { get; set; }
    public List<SelectListItem> GenderList { get; set; }

    [UIHint("BirthDate")]
    public DateOnly? BirthDay { get; set; }

    [Required(ErrorMessage = "Password")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "This {0} field is required")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string? Role { get; set; }
}
