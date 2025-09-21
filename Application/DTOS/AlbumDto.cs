using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;


namespace Application.DTOS
{
    public class AlbumDto
    {
        public int Id { get; set; }
        [Sortable, Searchable]
        public string ArName { get; set; }

        [Sortable, Searchable]
        public string EnName { get; set; }
        [Sortable, Searchable]
        public string? EnPicture { get; set; }
        [Sortable, Searchable]
        public string? ArPicture { get; set; }
        [Sortable, Searchable]
        public int DisplayOrder { get; set; }

        public bool Hidden { get; set; }
        [Sortable, Searchable]
        public DateTime? CreatedOnUtc { get; set; }
        [Sortable, Searchable]
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
