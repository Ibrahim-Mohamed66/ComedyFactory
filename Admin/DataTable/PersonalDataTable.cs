using Domain.Enums;
using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Admin.DataTable
{
    public class PersonalDataTable
    {
        [IncludeInReport(Order = 1)]
        [SearchableString]
        [Sortable]
        public string? Name { get; set; }
        [IncludeInReport(Order = 2)]
        [SearchableString]
        [Sortable]
        public string? Email { get; set; }
        [IncludeInReport(Order = 3)]
        [SearchableString]
        [Sortable]
        public string? Phone { get; set; }
        [IncludeInReport(Order = 4)]
        [SearchableString]
        [Sortable]
        [Display(Name = "Age")]
        public int? Age { get; set; }
        [IncludeInReport(Order = 5)]
        [SearchableString]
        [Sortable]
        [Display(Name = "City")]
        public string? City { get; set; }
        [IncludeInReport(Order = 6)]
        [SearchableString]
        [Sortable]
        [Display(Name = "Country")]
        public string? Country { get; set; }

        [IncludeInReport(Order = 7)]
        [SearchableString]
        [Sortable]
        [Display(Name = "Desire")]
        public string? Desire { get; set; }

        [IncludeInReport(Order = 8)]
        [SearchableString]
        public Gender Genders { get; set; }
        [IncludeInReport(Order = 9)]
        [SearchableString]
        [Sortable]
        public DateTime? CreatedOnUtc { get; set; }
    }
}
