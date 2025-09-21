using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly ReadDbContext _read;
        private readonly WriteDbContext _write;

        public CityRepository(ReadDbContext read, WriteDbContext write)
        {
            _read = read;
            _write = write;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(
            Expression<Func<City, bool>>? filter = null,
            Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null,
            Func<IQueryable<City>, IIncludableQueryable<City, object>>? include = null)
        {
            IQueryable<City> query = _read.Cities.Where(c=> !c.Deleted);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int id)
        {
            return await _read.Cities
                            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

        }

        public async Task<bool> AddCityAsync(City city)
        {
            await _write.Cities.AddAsync(city);
            return await _write.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateCityAsync(City city)
        {
            _write.Cities.Update(city);
            return await _write.SaveChangesAsync() > 0;

        }
        public async Task<City?> GetCityForUpdateAsync(int id)
        {
            return await _write.Cities.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
        }
        public async Task<bool> DeleteCityAsync(int id)
        {
            var entity = await _write.Cities.FindAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                _write.Cities.Update(entity);
               return await _write.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<int> GetCountAsync(Expression<Func<City, bool>>? filter = null)
        {
            IQueryable<City> query = _read.Cities;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<City?>> GetCitiesByCountryId(int countryId)
        {
            var cities = await _read.Cities
                .Where(c => c.CountryId == countryId && !c.Deleted)
                .ToListAsync();
            return cities;
        }
    }
}
