
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;

namespace Admin.ViewModels;

public class CountryViewModel
{
    public int CountryId { get; set; }
    [Sortable, Searchable]
    public string ArName { get; set; }

    [Sortable, Searchable]
    public string EnName { get; set; }

    [Sortable, Searchable]
    public int DisplayOrder { get; set; }

    public bool Hidden { get; set; }


}
