using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class Desire : BaseEntity
{
    [Required(ErrorMessage = "This {0} field is required")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    [Display(Name = "ArName")]
    public string ArName { get; set; }

    [Required(ErrorMessage = "This {0} field is required")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    [Display(Name = "EnName")]
    public string EnName { get; set; }

}
