using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class ActivityViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "ArName")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "EnName")]
        public string EnName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Arabic Content")]
        [UIHint("Editor")]
        public string ArContent { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "English Content")]
        [UIHint("Editor")]
        public string EnContent { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Icon")]
        [UIHint("FileUploader")]
        public string? Icon { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public int? DisplayOrder { get; set; }
        public bool Hidden { get; set; }

    }
}
