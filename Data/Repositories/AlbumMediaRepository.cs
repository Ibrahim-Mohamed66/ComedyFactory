using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace Data.Repositories
{
    public class AlbumMediaRepository : IAlbumMediaRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _readContext;
        public AlbumMediaRepository(WriteDbContext context, ReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }
        public async Task<bool> AddAlbumMediaAsync(AlbumMedia album)
        {
            await _context.AlbumMedias.AddAsync(album);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAlbumMediaAsync(int id)
        {
            var entity = await _context.AlbumMedias.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.AlbumMedias.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<IEnumerable<AlbumMedia>> GetAllAlbumMediaAsync(
            Expression<Func<AlbumMedia, bool>>? filter = null,
            Func<IQueryable<AlbumMedia>, IOrderedQueryable<AlbumMedia>>? orderBy = null,
            Func<IQueryable<AlbumMedia>, IIncludableQueryable<AlbumMedia, object>>? include = null)
        {
            IQueryable<AlbumMedia> query = _readContext.AlbumMedias.Where(c => !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<AlbumMedia>> GetAlbumMediasByAlbumIdAsync(
                                 int albumId,
                                 Expression<Func<AlbumMedia, bool>>? filter = null,
                                 Func<IQueryable<AlbumMedia>, IIncludableQueryable<AlbumMedia, object>>? include = null)
        {
            IQueryable<AlbumMedia> query = _context.AlbumMedias
                                                   .Where(a => a.AlbumId == albumId && !a.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }


        public async Task<bool> UpdateAlbumMediaAsync(AlbumMedia album)
        {
            _context.AlbumMedias.Update(album);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<AlbumMedia?> GetAlbumMediaByIdAsync(int id)
        {
            return await _context.AlbumMedias
                            .FirstOrDefaultAsync(a => a.Id == id && !a.Deleted);
        }

        public async Task<int> GetCountAsync(Expression<Func<AlbumMedia, bool>>? filter = null)
        {
            IQueryable<AlbumMedia> query = _readContext.AlbumMedias;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }
}

