using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class AlbumMediaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Media Type")]
        public MediaType MediaType { get; set; }

        [Display(Name = "Picture ")]
        [UIHint("PicUploader")]
        public string? Picture { get; set; }

        [UIHint("Video")]
        [Display(Name = "Youtube Link")]
        public string? YouTubeLink { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        public bool Hidden { get; set; }

        public int? AlbumId { get; set; }
    }
}
