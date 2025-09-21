using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class AlbumViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Arabic Name")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "English Name")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string EnName { get; set; }

        [Display(Name = "English Picture")]
        [UIHint("PicUploader")]
        public string? EnPicture { get; set; }

        [Display(Name = "Arabic Picture")]
        [UIHint("PicUploader")]
        public string? ArPicture { get; set; }
        public bool Hidden { get; set; }
        [Required]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
