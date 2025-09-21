
using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOS
{
    public class ActivityDto
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 2)]
        [Sortable]
        public string ArName { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 3)]
        [Sortable]
        public string EnName { get; set; }


        [IncludeInReport(Order = 4)]
        [Sortable]
        public string ArContent { get; set; }

        [IncludeInReport(Order = 5)]
        [Sortable]
        public string EnContent { get; set; }

        [IncludeInReport(Order = 6)]
        [SearchableString]
        [Sortable]
        public string? Icon { get; set; }

        [IncludeInReport(Order = 7)]
        [SearchableString]
        [Sortable]
        public DateTime? CreatedOnUtc { get; set; }
        [Sortable]
        public DateTime? UpdatedOnUtc { get; set; }
        [Sortable]
        public int? DisplayOrder { get; set; }
        public bool Hidden { get; set; }
    }
}
