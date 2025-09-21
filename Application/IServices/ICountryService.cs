
using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;

namespace Application.IServices
{
    public interface ICountryService
    {
        Task<JqueryDataTablesPagedResults<CountryDto>> GetCountiesDataTableAsync(JqueryDataTablesParameters table);
        Task<CountryDto?> GetCountryByIdAsync(int id);
        Task<CountryDto> CreateCountryAsync(CountryDto countryDto);
        Task<CountryDto?> UpdateCountryAsync(int id, CountryDto countryDto);
        Task<bool> DeleteCountryAsync(int id);
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync();
    }
}
