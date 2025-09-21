using Domain.Enums;
using IOC.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    public class PersonalDataViewModel
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "NameRequired")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "NameLength")]
        public string Name { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "PhoneRequired")]
        [Phone(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "PhoneInvalid")]
        public string Phone { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "EmailInvalid")]
        [StringLength(255,
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "EmailLength")]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "AgeRequired")]
        [Range(13, 120,
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "AgeRange")]
        public int Age { get; set; }
        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "CityRequired")]
        public int? CityId { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "CountryRequired")]
        public int? CountryId { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "GenderRequired")]
        [Display(Name = "Gender", ResourceType = typeof(SharedResource))]
        public Gender? Genders { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "DesireRequired")]
        public int? DesireId { get; set; }

        // UI helpers
        public IEnumerable<SelectListItem>? Countries { get; set; }
        public IEnumerable<SelectListItem>? GendersList { get; set; }
        public IEnumerable<SelectListItem>? Cities { get; set; }
        public IEnumerable<SelectListItem>? Desires { get; set; }
        public IEnumerable<SelectListItem>? GenderOptions { get; set; }
    }
}
