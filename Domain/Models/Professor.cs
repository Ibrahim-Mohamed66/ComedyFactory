using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Professor : BaseEntity
    {

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "ArName")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "EnName")]
        public string EnName { get; set; }


        [Display(Name = "Arabic Picture")]
        [UIHint("PicUploader")]
        public string ArPicture { get; set; }

        [Display(Name = "English Picture")]
        [UIHint("PicUploader")]
        public string EnPicture { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Arabic Content")]
        [UIHint("Editor")]
        public string ArContent { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "English Content")]
        [UIHint("Editor")]
        public string EnContent { get; set; }

        [Display(Name = "MasterCategoryId")]
        public int? MasterCategoryId { get; set; }
        [ValidateNever]
        public MasterCategory? MasterCategory { get; set; }

    }
}