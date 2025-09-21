using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOS;

public class ConfigurationDto
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

    [DisplayName("Facebook Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? Facebook { get; set; }


    [DisplayName("Twitter Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? Twitter { get; set; }


    [DisplayName("Youtube Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? Youtube { get; set; }


    [DisplayName("LinkedIn Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? LinkedIn { get; set; }


    [DisplayName("Instagram Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? Instagram { get; set; }

    [DisplayName("Tiktok Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? Tiktok { get; set; }

    [DisplayName("Snapchat Link")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? Snapchat { get; set; }


    [DisplayName("Default Email Address")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? DefaultEmailAddress { get; set; }


    [DisplayName("Default Email Name")]
    [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
    public string? DefaultEmailName { get; set; }

    [DisplayName("Email Sender")]

    public string? EmailSender { get; set; }

    [DisplayName("Password Email Sender")]
    public string? PasswordEmailSender { get; set; }

    [DisplayName("Port")]
    public int? Port { get; set; }

    [DisplayName("Use SSL")]
    [DefaultValue(false)]
    public bool UseSSL { get; set; }

    [DisplayName("Host")]
    public string? Host { get; set; }


    [DisplayName("Google Analytics Code")]
    [MaxLength(1000, ErrorMessage = "Maximum characters is 1000 character")]
    [UIHint("MiniEditor")]
    public string? GoogleAnalytics { get; set; }

    [DisplayName("Google Analytics Emails")]
    [MaxLength(1000, ErrorMessage = "Maximum characters is 1000 character")]
    public string? GoogleAnalyticsEmail { get; set; }

    [DisplayName("SEO Scripts")]
    [UIHint("Textarea")]
    public string? SEOScripts { get; set; }

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
