using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IAlbumRepository
    {
        Task<bool> AddAlbumAsync(Album album);
        Task<bool> UpdateAlbumAsync(Album album);
        Task<bool> DeleteAlbumAsync(int albumId);
        Task<Album?> GetAlbumByIdAsync(int albumId);
        Task<IEnumerable<Album>> GetAllAlbumsAsync(
                    Expression<Func<Album, bool>>? filter = null,
                    Func<IQueryable<Album>, IOrderedQueryable<Album>>? orderBy = null,
                    Func<IQueryable<Album>, IIncludableQueryable<Album, object>>? include = null);
        Task<int>GetCountAsync(Expression<Func<Album, bool>>? filter = null);
    }
}
