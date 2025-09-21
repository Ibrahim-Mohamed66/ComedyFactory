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
    public class PersonalDataService : IPersonalDataService
    {
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly IMapper _mapper;
        public PersonalDataService(IPersonalDataRepository personalDataRepository, IMapper mapper)
        {
            _personalDataRepository = personalDataRepository;
            _mapper = mapper;
        }
        public async Task<PersonalDataDto> CreatePersonDataAsync(PersonalDataDto personalDataDto)
        {
            var personalData = _mapper.Map<PersonalData>(personalDataDto);
            await _personalDataRepository.AddPersonalDataAsync(personalData);
            return _mapper.Map<PersonalDataDto>(personalData);
        }

        public async Task<PersonalDataDto?> GetPersonDataByIdAsync(int id)
        {
            var personalData = await _personalDataRepository.GetPersonalDataByIdAsync(id);
            return _mapper.Map<PersonalDataDto?>(personalData);
        }

        public async Task<JqueryDataTablesPagedResults<PersonalDataDto>> GetPersonDatasDataTableAsync(JqueryDataTablesParameters table)
        {

            Expression<Func<PersonalData, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = p =>
                    (p.Name != null && p.Name.ToLower().Contains(search)) ||
                    (p.Email != null && p.Email.ToLower().Contains(search)) ||
                    (p.Phone != null && p.Phone.ToLower().Contains(search)) ||
                    (p.City != null && p.City.EnName.ToLower().Contains(search)) ||
                    (p.Desire != null && p.Desire.ArName.ToLower().Contains(search)) || 
                    (p.Country != null && p.Country.EnName.ToLower().Contains(search)) ||
                    (p.Country != null && p.Country.ArName.ToLower().Contains(search)) ||
                    (p.Age != null && p.Age.ToString() == search) ||
                    (p.Desire != null && p.Desire.EnName.ToLower().Contains(search));
            }
            var person = (await _personalDataRepository.GetAllPersonalDatasAsync(
                filter,
                include: q => q.Include(p => p.Desire!)
                               .Include(p => p.Country!)
                               .Include(p => p.City!)
            )).AsQueryable();

            // Apply search and sorting to the entity query first
            var mappedResult = _mapper.ProjectTo<PersonalDataDto>(person);
            mappedResult = SearchOptionsProcessor<PersonalDataDto, PersonalDataDto>.Apply(mappedResult, table.Columns);
            mappedResult = SortOptionsProcessor<PersonalDataDto, PersonalDataDto>.Apply(mappedResult, table);

            var totalRecords = person.Count();
            var filteredRecords = await _personalDataRepository.GetCountAsync(filter);

            // Skip and take on the entity level, then map to DTO
            var entities = mappedResult
                .Skip(table.Start)
                .Take(table.Length)
                .ToList();

            var data = _mapper.Map<List<PersonalDataDto>>(entities);

            return new JqueryDataTablesPagedResults<PersonalDataDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }
    }
}
