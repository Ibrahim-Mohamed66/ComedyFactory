using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class DesireRepository:IDesireRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _readContext;
        public DesireRepository(WriteDbContext context, ReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }
        public async Task<IEnumerable<Desire>> GetAllDesiresAsync(
            Expression<Func<Desire, bool>>? filter = null,
            Func<IQueryable<Desire>, IOrderedQueryable<Desire>>? orderBy = null,
            Func<IQueryable<Desire>, IIncludableQueryable<Desire, object>>? include = null)
        {
            IQueryable<Desire> query = _readContext.Desires.Where(c => !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Desire?> GetDesireByIdAsync(int id)
        {
            return await _readContext.Desires
                            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        }

        public async Task<bool> AddDesireAsync(Desire Desire)
        {
            await _context.Desires.AddAsync(Desire);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateDesireAsync(Desire Desire)
        {
            _context.Desires.Update(Desire);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<Desire?> GetDesireForUpdateAsync(int id)
        {
            return await _context.Desires.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }
        public async Task<bool> DeleteDesireAsync(int id)
        {
            var entity = await _context.Desires.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.Desires.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<int> GetCountAsync(Expression<Func<Desire, bool>>? filter = null)
        {
            IQueryable<Desire> query = _context.Desires;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }
}
