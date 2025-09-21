using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace Data.Repositories
{
    public class MasterCategoryRepository: IMasterCategoryRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _read;
        public MasterCategoryRepository(WriteDbContext context, ReadDbContext read)
        {
            _context = context;
            _read = read;
        }
        public async Task<IEnumerable<MasterCategory>> GetAllMasterCategoriesAsync(
            Expression<Func<MasterCategory, bool>>? filter = null,
            Func<IQueryable<MasterCategory>, IOrderedQueryable<MasterCategory>>? orderBy = null,
            Func<IQueryable<MasterCategory>, IIncludableQueryable<MasterCategory, object>>? include = null)
        {
            IQueryable<MasterCategory> query = _read.MasterCategories.Where(c => !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<MasterCategory?> GetMasterCategoryByIdAsync(int id)
        {
            return await _read.MasterCategories
                            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        }

        public async Task<bool> AddMasterCategoryAsync(MasterCategory masterCategory)
        {
            await _context.MasterCategories.AddAsync(masterCategory);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateMasterCategoryAsync(MasterCategory masterCategory)
        {
            _context.MasterCategories.Update(masterCategory);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<MasterCategory?> GetMasterCategoryForUpdateAsync(int id)
        {
            return await _context.MasterCategories.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }
        public async Task<bool> DeleteMasterCategoryAsync(int id)
        {
            var entity = await _context.MasterCategories.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.MasterCategories.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<int> GetCountAsync(Expression<Func<MasterCategory, bool>>? filter = null)
        {
            IQueryable<MasterCategory> query = _context.MasterCategories;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }
}
