using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories.IRepositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System.Linq.Expressions;

namespace Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<CountryDto> CreateCountryAsync(CountryDto countryDto)
        {
            var entity = _mapper.Map<Country>(countryDto);
            

            var result = await _countryRepository.AddCountryAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create country");
            }
            return _mapper.Map<CountryDto>(entity);
        }

        public async Task<bool> DeleteCountryAsync(int id)
        {
            return await _countryRepository.DeleteCountryAsync(id);
        }

        public async Task<JqueryDataTablesPagedResults<CountryDto>> GetCountiesDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<Country, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }
           

            var countries = (await _countryRepository.GetAllCountriesAsync(filter)).AsQueryable();
            countries =  SearchOptionsProcessor<CountryDto, Country>.Apply(countries, table.Columns);
            countries = SortOptionsProcessor<CountryDto, Country>.Apply(countries, table);


            var totalRecords = countries.Count();
            var filteredRecords = await _countryRepository.GetCountAsync(filter);

            var data = countries
                .Skip(table.Start)
                .Take(table.Length)
                .Select(c => _mapper.Map<CountryDto>(c))
                .ToList();

            return new JqueryDataTablesPagedResults<CountryDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync()
        {
            var entities = await _countryRepository.GetAllCountriesAsync(c => !c.Deleted);
            return _mapper.Map<IEnumerable<CountryDto>>(entities);
        }

        public async Task<CountryDto?> GetCountryByIdAsync(int id)
        {
            var entity = await _countryRepository.GetCountryByIdAsync(id);
            return entity == null ? null : _mapper.Map<CountryDto>(entity);
        }

        public async Task<CountryDto?> UpdateCountryAsync(int id, CountryDto countryDto)
        {
            var entity = await _countryRepository.GetCountryForUpdateAsync(id); 
            if (entity == null) return null;

            _mapper.Map(countryDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _countryRepository.UpdateCountryAsync(entity); 
            return _mapper.Map<CountryDto>(entity);
        }

    }
}
