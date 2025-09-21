using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class MasterCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "ArName")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "EnName")]
        public string EnName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Hidden { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
