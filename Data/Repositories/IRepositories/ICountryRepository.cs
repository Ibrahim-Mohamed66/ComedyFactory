using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories.IRepositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync(
            Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            Func<IQueryable<Country>, IIncludableQueryable<Country, object>>? include = null
        );

        Task<Country?> GetCountryByIdAsync(int id);
        Task<bool> AddCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(int id);
        Task<int> GetCountAsync(Expression<Func<Country, bool>>? filter = null);
        Task<Country?> GetCountryForUpdateAsync(int id);
    }
}
