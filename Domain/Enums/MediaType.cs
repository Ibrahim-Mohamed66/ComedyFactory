
using System.ComponentModel.DataAnnotations;


namespace Domain.Enums
{
    public enum MediaType
    {
        [Display(Name = "Photo")]
        Photo = 1,
        [Display(Name = "Youtube")]
        YoutubeURL,
        [Display(Name = "None")]
        None,
    }
}
