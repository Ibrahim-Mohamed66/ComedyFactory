using Data.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace Data.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _readContext;
        public AlbumRepository(WriteDbContext context, ReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }
        public async Task<bool> AddAlbumAsync(Album album)
        {
            await _context.Albums.AddAsync(album);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAlbumAsync(int id)
        {
            var entity = await _context.Albums.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.Albums.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<IEnumerable<Album>> GetAllAlbumsAsync(
            Expression<Func<Album, bool>>? filter = null,
            Func<IQueryable<Album>, IOrderedQueryable<Album>>? orderBy = null,
            Func<IQueryable<Album>, IIncludableQueryable<Album, object>>? include = null)
        {
            IQueryable<Album> query = _readContext.Albums.Where(c => !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(int id)
        {
            return await _context.Albums
                            .FirstOrDefaultAsync(a => a.Id == id && !a.Deleted);

        }

        public async Task<bool> UpdateAlbumAsync(Album album)
        {
            _context.Albums.Update(album);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<int> GetCountAsync(Expression<Func<Album, bool>>? filter = null)
        {
            IQueryable<Album> query = _readContext.Albums;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }
}
