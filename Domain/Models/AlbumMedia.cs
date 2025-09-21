using Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models
{
    public class AlbumMedia : BaseEntity
    {
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

        public int? AlbumId { get; set; }
        [ForeignKey("AlbumId")]
        [ValidateNever]
        public Album? Album { get; set; }

    }
}