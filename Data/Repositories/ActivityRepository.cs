using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _readContext;
        public ActivityRepository(WriteDbContext context, ReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync(
            Expression<Func<Activity, bool>>? filter = null,
            Func<IQueryable<Activity>, IOrderedQueryable<Activity>>? orderBy = null,
            Func<IQueryable<Activity>, IIncludableQueryable<Activity, object>>? include = null)
        {
            IQueryable<Activity> query = _readContext.Activities.Where(c => !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Activity?> GetActivityByIdAsync(int id)
        {
            return await _readContext.Activities
                            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        }

        public async Task<bool> AddActivityAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateActivityAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<Activity?> GetActivityForUpdateAsync(int id)
        {
            return await _context.Activities.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }
        public async Task<bool> DeleteActivityAsync(int id)
        {
            var entity = await _context.Activities.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _context.Activities.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<int> GetCountAsync(Expression<Func<Activity, bool>>? filter = null)
        {
            IQueryable<Activity> query = _context.Activities;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

    }
}
