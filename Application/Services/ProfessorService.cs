using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories;
using Data.Repositories.IRepositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Application.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepo;
        private readonly IMapper _mapper;
        public ProfessorService(IMapper mapper, IProfessorRepository professorRepo)
        {
            _mapper = mapper;
            _professorRepo = professorRepo;
        }
        public async Task<ProfessorDto> CreateProfessorAsync(ProfessorDto professorDto)
        {
            var entity = _mapper.Map<Professor>(professorDto);

            var result = await _professorRepo.AddProfessorAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create Professor");
            }
            return _mapper.Map<ProfessorDto>(entity);
        }

        public async Task<bool> DeleteProfessorAsync(int id)
        {
            return await _professorRepo.DeleteProfessorAsync(id);
        }

        public async Task<JqueryDataTablesPagedResults<ProfessorDto>> GetProfessorsDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<Professor, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }
            // Fix: Specify the navigation property name for Include (e.g., "Country")
            var professors = (await _professorRepo.GetAllProfessorsAsync(
                filter,
                include: c => c.Include(Professor => Professor.MasterCategory)
            )).AsQueryable();

            var mappedResult = _mapper.ProjectTo<ProfessorDto>(professors);
            mappedResult = SearchOptionsProcessor<ProfessorDto, ProfessorDto>.Apply(mappedResult, table.Columns);
            mappedResult = SortOptionsProcessor<ProfessorDto, ProfessorDto>.Apply(mappedResult, table);

            var totalRecords = mappedResult.Count();
            var filteredRecords = await _professorRepo.GetCountAsync(filter);

            var data = mappedResult
                .Skip(table.Start)
                .Take(table.Length)
                .ToList();

            return new JqueryDataTablesPagedResults<ProfessorDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }

        public async Task<ProfessorDto?> GetProfessorByIdAsync(int id)
        {
            var entity = await _professorRepo.GetProfessorByIdAsync(id);
            return entity == null ? null : _mapper.Map<ProfessorDto>(entity);
        }

        public async Task<ProfessorDto?> UpdateProfessorAsync(int id, ProfessorDto professorDto)
        {
            var entity = await _professorRepo.GetProfessorForUpdateAsync(id);
            if (entity == null) return null;

            _mapper.Map(professorDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _professorRepo.UpdateProfessorAsync(entity);
            return _mapper.Map<ProfessorDto>(entity);
        }

        public async Task<IEnumerable<ProfessorDto>> GetAllProfessorsAsync()
        {
            var entities = await _professorRepo.GetAllProfessorsAsync();
            return _mapper.Map<IEnumerable<ProfessorDto>>(entities);
        }
    }
}
