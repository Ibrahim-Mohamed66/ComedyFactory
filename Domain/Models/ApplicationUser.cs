using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "FullName")]
        public string? FullName { get; set; }

        public string? CountryCode { get; set; }


        public string Mobile { get; set; }

        public string? Picture { get; set; }



        public string? NationalityId { get; set; }
        public int? RegistrationId { get; set; }

        public DateTime? RefreshTokenExpiryUTC { get; set; }


        public bool Deleted { get; set; }

        public bool Active { get; set; }

        public string? MobileAppId { get; set; }

        public string? Language { get; set; }


        public string? LoginDevice { get; set; }

        public string? LoginIPAddress { get; set; }

        public string? LoginIpCity { get; set; }

        public string? LoginIpCountry { get; set; }

        public string? LoginLocation { get; set; }


        public string? RegisterDevice { get; set; }

        public string? RegisterIPAddress { get; set; }

        public string? RegisterIpCity { get; set; }

        public string? RegisterIpCountry { get; set; }

        public string? RegisterLocation { get; set; }

        [DefaultValue(false)]
        public bool IsMobileDevice { get; set; }

        public string? Headers { get; set; }

        public string? EndpointArn { get; set; }

        public bool HasTopic { get; set; }

        [DefaultValue(false)]
        public bool EnableNotification { get; set; }

        public string? TopicArn { get; set; }

        public DateOnly? BirthDay { get; set; }
        public Gender? Gender { get; set; }


        public int? Age { get; set; }
        public double? Weight { get; set; }
        public double? TotalScorePoint { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated On Utc")]
        public DateTime? UpdatedOnUtc { get; set; }
        [ValidateNever]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
