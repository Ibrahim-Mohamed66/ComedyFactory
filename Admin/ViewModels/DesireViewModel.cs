using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;

namespace Admin.ViewModels
{
    public class DesireViewModel
    {
        public int Id { get; set; }
        public string ArName { get; set; }
        public string EnName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Hidden { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
