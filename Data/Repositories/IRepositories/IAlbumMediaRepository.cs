using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.IRepositories
{
    public interface IAlbumMediaRepository
    {

        Task<bool> AddAlbumMediaAsync(AlbumMedia album);
        Task<bool> UpdateAlbumMediaAsync(AlbumMedia album);
        Task<bool> DeleteAlbumMediaAsync(int albumId);
        Task<IEnumerable<AlbumMedia>> GetAlbumMediasByAlbumIdAsync(
                                 int albumId,
                                 Expression<Func<AlbumMedia, bool>>? filter = null,
                                 Func<IQueryable<AlbumMedia>, IIncludableQueryable<AlbumMedia, object>>? include = null);
        Task<AlbumMedia?> GetAlbumMediaByIdAsync(int id);
        Task<IEnumerable<AlbumMedia>> GetAllAlbumMediaAsync(
                    Expression<Func<AlbumMedia, bool>>? filter = null,
                    Func<IQueryable<AlbumMedia>, IOrderedQueryable<AlbumMedia>>? orderBy = null,
                    Func<IQueryable<AlbumMedia>, IIncludableQueryable<AlbumMedia, object>>? include = null);
        Task<int> GetCountAsync(Expression<Func<AlbumMedia, bool>>? filter = null);
    }
}
