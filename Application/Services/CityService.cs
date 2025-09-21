using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories.IRepositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<CityDto> CreateCityAsync(CityDto cityDto)
        {
            var entity = _mapper.Map<City>(cityDto);

            var result = await _cityRepository.AddCityAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create City");
            }
            return _mapper.Map<CityDto>(entity);
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            return await _cityRepository.DeleteCityAsync(id);
        }

        public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
        {
            var cities = await _cityRepository.GetAllCitiesAsync(c => !c.Deleted, include: c => c.Include(city => city.Country));
            return _mapper.Map<IEnumerable<CityDto>>(cities);

        }

        public async Task<JqueryDataTablesPagedResults<CityDto>> GetCitiesDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<City, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }
            // Fix: Specify the navigation property name for Include (e.g., "Country")
            var cities = (await _cityRepository.GetAllCitiesAsync(
                filter,
                include: c => c.Include(city => city.Country)
            )).AsQueryable();

            var mappedResult =  _mapper.ProjectTo<CityDto>(cities);
            mappedResult = SearchOptionsProcessor<CityDto, CityDto>.Apply(mappedResult, table.Columns);
            mappedResult = SortOptionsProcessor<CityDto, CityDto>.Apply(mappedResult, table);

            var totalRecords = mappedResult.Count();
            var filteredRecords = await _cityRepository.GetCountAsync(filter);

            var data = mappedResult
                .Skip(table.Start)
                .Take(table.Length)

                .ToList();

            return new JqueryDataTablesPagedResults<CityDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }

        public async Task<CityDto?> GetCityByIdAsync(int id)
        {
            var entity = await _cityRepository.GetCityByIdAsync(id);
            return entity == null ? null : _mapper.Map<CityDto>(entity);
        }

        public async Task<CityDto?> UpdateCityAsync(int id, CityDto cityDto)
        {
            var entity = await _cityRepository.GetCityForUpdateAsync(id); 
            if (entity == null) return null;

            _mapper.Map(cityDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _cityRepository.UpdateCityAsync(entity); 
            return _mapper.Map<CityDto>(entity);
        }

        public async Task<IEnumerable<CityDto?>> GetCitiesByCountryId(int countryId)
        {
            var entities = await _cityRepository.GetCitiesByCountryId(countryId);
            return entities == null ? null : _mapper.Map<IEnumerable<CityDto>>(entities);
        }

    }
}
