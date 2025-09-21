using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class CityViewModel
    {
        public int CityId { get; set; }
        [Sortable, Searchable]
        public string ArName { get; set; }

        [Sortable, Searchable]
        public string EnName { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select a country")]
        public int? CountryId { get; set; } 

        [ValidateNever]
        public IEnumerable<SelectListItem> Countries { get; set; }

        [Sortable, Searchable]
        public int DisplayOrder { get; set; }

        public bool Hidden { get; set; }
    }
}
