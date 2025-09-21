using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOS
{
    public class ProfessorDto
    {
        [IncludeInReport(Order = 1)]
        public int Id { get; set; }


        [SearchableString()]
        [IncludeInReport(Order = 2)]
        [Sortable]
        public string ArName { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 3)]
        [Sortable]
        public string EnName { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string ArPicture { get; set; }

        [SearchableString()]
        [IncludeInReport(Order = 3)]
        public string EnPicture { get; set; }

        [IncludeInReport(Order = 5)]
        [Sortable]
        public int DisplayOrder { get; set; }

        [IncludeInReport(Order = 6)]

        public bool Hidden { get; set; }


        [IncludeInReport(Order = 7)]
        [SearchableString]
        [Sortable]
        public DateTime? CreatedOnUtc { get; set; }

        [IncludeInReport(Order = 8)]
        [SearchableString]
        [Sortable]
        public DateTime? UpdatedOnUtc { get; set; }
        [IncludeInReport(Order = 9)]
        [SearchableString]
        [Sortable]
        public string? MasterCategory { get; set; }
        public int? MasterCategoryId { get; set; }

        public string ArContent { get; set; }
        public string EnContent { get; set; }
    }
}
