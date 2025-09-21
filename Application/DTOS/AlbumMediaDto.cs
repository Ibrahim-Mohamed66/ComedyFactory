using Domain.Enums;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class AlbumMediaDto
    {
        public int Id { get; set; }
        [Sortable, Searchable]
        public MediaType MediaType { get; set; }

        [Sortable, Searchable]

        public string? Picture { get; set; }

        [Sortable, Searchable]

        public string? YouTubeLink { get; set; }

        public int? AlbumId { get; set; }
        [Sortable, Searchable]
        public string? AlbumName { get; set; }
        [Sortable, Searchable]
        public int DisplayOrder { get; set; }

        public bool Hidden { get; set; }
        [Sortable, Searchable]
        public DateTime? CreatedOnUtc { get; set; }
        [Sortable, Searchable]
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
