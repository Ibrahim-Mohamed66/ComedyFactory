using Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class BlockViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "Arabic Name")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "English Name")]
        public string EnName { get; set; }


        [Display(Name = "Arabic Content")]
        [UIHint("Editor")]
        public string? ArContent { get; set; }

        [Display(Name = "English Content")]
        [UIHint("Editor")]
        public string? EnContent { get; set; }


        [Display(Name = "Arabic Picture")]
        [UIHint("PicUploader")]
        public string? ArPicture { get; set; }

        [Display(Name = "English Picture")]
        [UIHint("PicUploader")]
        public string? EnPicture { get; set; }

        public BlockType BlockType { get; set; }
        [Display(Name = "Arabic Description")]
        [UIHint("Editor")]
        public string? ArDescription { get; set; }
        [Display(Name = "English Description")]
        [UIHint("Editor")]
        public string? EnDescription { get; set; }


        [DisplayName("Ar Youtube Video")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? ArYoutubeVideo { get; set; }

        [DisplayName("En Youtube Video")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        public string? EnYoutubeVideo { get; set; }

        public bool Deleted { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated On Utc")]
        public DateTime? UpdatedOnUtc { get; set; }

        [Display(Name = "Display Order")]
        [DefaultValue(10)]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Hidden")]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool Hidden { get; set; }

    }
}
