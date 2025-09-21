using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class PersonalDataRepository:IPersonalDataRepository
    {
        private readonly ReadDbContext _read;
        private readonly WriteDbContext _context;
        public PersonalDataRepository(ReadDbContext read, WriteDbContext context)
        {
            _read = read;
            _context = context;
        }

        public async Task<bool> AddPersonalDataAsync(PersonalData PersonalData)
        {
            await _context.PersonalDatas.AddAsync(PersonalData);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PersonalData>> GetAllPersonalDatasAsync(Expression<Func<PersonalData, bool>>? filter = null, Func<IQueryable<PersonalData>, IOrderedQueryable<PersonalData>>? orderBy = null, Func<IQueryable<PersonalData>, IIncludableQueryable<PersonalData, object>>? include = null)
        {
            var query = _read.PersonalDatas.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().Where(p =>  !p.Deleted).ToListAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<PersonalData, bool>>? filter = null)
        {
            var query = _read.PersonalDatas.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var count = await query.CountAsync(p => !p.Deleted);
            return count;
        }

        public async Task<PersonalData?> GetPersonalDataByIdAsync(int id)
        {
            return await _context.PersonalDatas.FirstOrDefaultAsync(p => !p.Deleted);

        }

        public async Task<PersonalData?> GetPersonalDataForUpdateAsync(int id)
        {
            return await _context.PersonalDatas.FirstOrDefaultAsync(p => !p.Deleted);
        }
    }
}
