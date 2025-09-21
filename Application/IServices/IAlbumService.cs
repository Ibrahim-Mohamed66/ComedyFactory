using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;

namespace Application.IServices
{
    public interface IAlbumService
    {
        Task<JqueryDataTablesPagedResults<AlbumDto>> GetAlbumsDataTableAsync(JqueryDataTablesParameters table);
        Task<AlbumDto?> GetAlbumByIdAsync(int id);
        Task<AlbumDto> CreateAlbumAsync(AlbumDto album);
        Task<AlbumDto?> UpdateAlbumAsync(int id, AlbumDto album);
        Task<bool> DeleteAlbumAsync(int id);
        Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync();

    }
}
