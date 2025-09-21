using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Album : BaseEntity
    {
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
        public ICollection<AlbumMedia>? AlbumMedias { get; set; }

    }
}
