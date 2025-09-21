using Domain.Enums;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Admin.DataTable
{
    public class AlbumMediaDataTable
    {
        [Sortable,Searchable]
        public MediaType MediaType { get; set; }
        [Sortable, Searchable]
        public string? Picture { get; set; }

        [Sortable, Searchable]
        public string? YouTubeLink { get; set; }
        [Sortable, Searchable]
        public int DisplayOrder { get; set; }
        public int? AlbumId { get; set; }

        [Sortable, Searchable]
        public DateTime? CreatedOnUtc { get; set; }
        [Sortable, Searchable]
        public DateTime? UpdatedOnUtc { get; set; }


    }
}
