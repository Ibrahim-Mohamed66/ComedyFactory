
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace Domain.Models
{
    public class City : BaseEntity
    {

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Arabic Name")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "English Name")]
        public string EnName { get; set; }

        [ValidateNever]
        public Country? Country { get; set; }

    }
}
