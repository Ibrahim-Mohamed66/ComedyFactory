using Domain.Enums;
using Domain.Models;
using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;

namespace Application.DTOS
{
    public class PersonalDataDto
    {
        public int Id { get; set; }
        [IncludeInReport(Order = 1)]
        [SearchableString]
        [Sortable]
        public string? Name { get; set; }
        [IncludeInReport(Order = 2)]
        [SearchableString]
        [Sortable]
        public string? Phone { get; set; }
        [IncludeInReport(Order = 3)]
        [SearchableString]
        [Sortable]
        public string? Email { get; set; }
        [IncludeInReport(Order = 4)]
        [SearchableString]
        [Sortable]
        public int? Age { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string? Photo { get; set; }

        [IncludeInReport(Order = 5)]
        [SearchableString]
        [Sortable]
        public Gender Genders { get; set; }
        [IncludeInReport(Order = 6)]
        [SearchableString]
        [Sortable]
        public DateTime? CreatedOnUtc { get; set; }
        public int? DesireId { get; set; }

        [IncludeInReport(Order = 7)]
        [SearchableString]
        [Sortable]
        public string? City { get; set; }
        [IncludeInReport(Order = 8)]
        [SearchableString]
        [Sortable]
        public string? Country { get; set; }
        [IncludeInReport(Order = 9)]
        [SearchableString]
        [Sortable]
        public string? Desire { get; set; }
    }
}

