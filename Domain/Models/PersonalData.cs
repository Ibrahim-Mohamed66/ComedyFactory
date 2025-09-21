
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class PersonalData : BaseEntity
    {
        [Display(Name = "Full name")]
        public string? Name { get; set; }


        [Display(Name = "Phone")]
        public string? Phone { get; set; }


        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Display(Name = "Personal photo")]
        public string? Photo { get; set; }

        public Gender Genders { get; set; }
        public Country? Country { get; set; }

        public City? City { get; set; }

        public int? DesireId { get; set; }
        public Desire? Desire { get; set; }
    }
}
