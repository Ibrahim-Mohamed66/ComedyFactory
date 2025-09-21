using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IAlbumMediaService
    {
        Task<JqueryDataTablesPagedResults<AlbumMediaDto>> GetAlbumMediasDataTableAsync(int albumId, JqueryDataTablesParameters table);
        Task<AlbumMediaDto?> GetAlbumMediaByIdAsync(int id);
        Task<AlbumMediaDto> CreateAlbumMediaAsync(AlbumMediaDto albumMedia);
        Task<AlbumMediaDto?> UpdateAlbumMediaAsync(int id, AlbumMediaDto albumMedia);
        Task<bool> DeleteAlbumMediaAsync(int id);
        Task<IEnumerable<AlbumMediaDto>> GetAllAlbumMediasAsync();
    }
}
