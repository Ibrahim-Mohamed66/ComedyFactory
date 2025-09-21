
using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;

namespace Application.IServices
{
    public interface ICityService
    {
        Task<JqueryDataTablesPagedResults<CityDto>> GetCitiesDataTableAsync(JqueryDataTablesParameters table);
        Task<CityDto?> GetCityByIdAsync(int id);
        Task<CityDto> CreateCityAsync(CityDto cityDto);
        Task<CityDto?> UpdateCityAsync(int id, CityDto cityDto);
        Task<bool> DeleteCityAsync(int id);

        Task<IEnumerable<CityDto>> GetAllCitiesAsync();

        Task<IEnumerable<CityDto?>> GetCitiesByCountryId(int countryId);

    }
}
