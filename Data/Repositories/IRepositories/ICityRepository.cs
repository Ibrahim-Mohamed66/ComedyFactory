using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories.IRepositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync(
            Expression<Func<City, bool>>? filter = null,
            Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null,
            Func<IQueryable<City>, IIncludableQueryable<City, object>>? include = null
        );

        Task<City?> GetCityByIdAsync(int id);
        Task<bool> AddCityAsync(City city);
        Task<bool> UpdateCityAsync(City city);
        Task<bool> DeleteCityAsync(int id);
        Task<int> GetCountAsync(Expression<Func<City, bool>>? filter = null);
        Task<City?> GetCityForUpdateAsync(int id);
        Task<IEnumerable<City?>> GetCitiesByCountryId(int countryId);
    }
}
