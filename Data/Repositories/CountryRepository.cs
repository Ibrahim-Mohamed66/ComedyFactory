using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ReadDbContext _read;
        private readonly WriteDbContext _write;

        public CountryRepository(ReadDbContext read, WriteDbContext write)
        {
            _read = read;
            _write = write;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync(
            Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            Func<IQueryable<Country>, IIncludableQueryable<Country, object>>? include = null)
        {
            IQueryable<Country> query = _read.Countries.Where(c=> !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Country?> GetCountryByIdAsync(int id)
        {
            return await _read.Countries
                            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        }

        public async Task<bool> AddCountryAsync(Country country)
        {
            await _write.Countries.AddAsync(country);
            return await _write.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            _write.Countries.Update(country);
            return await _write.SaveChangesAsync() > 0;

        }
        public async Task<Country?> GetCountryForUpdateAsync(int id)
        {
            return await _write.Countries.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }
        public async Task<bool> DeleteCountryAsync(int id)
        {
            var entity = await _write.Countries.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _write.Countries.Update(entity);
               return await _write.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<int> GetCountAsync(Expression<Func<Country, bool>>? filter = null)
        {
            IQueryable<Country> query = _read.Countries;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }
    }
}
