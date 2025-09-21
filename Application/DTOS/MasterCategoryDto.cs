using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;

namespace Application.DTOS
{
    public class MasterCategoryDto
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
    }
}
