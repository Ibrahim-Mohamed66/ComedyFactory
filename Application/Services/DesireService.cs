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
    public class DesireService : IDesireService
    {
        private readonly IDesireRepository _desireRepo;
        private readonly IMapper _mapper;
        public DesireService(IDesireRepository desireRepo, IMapper mapper)
        {
            _desireRepo = desireRepo;
            _mapper = mapper;
        }
        public async Task<DesireDto> CreateDesireAsync(DesireDto DesireDto)
        {
            var entity = _mapper.Map<Desire>(DesireDto);

            var result = await _desireRepo.AddDesireAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create Desire");
            }
            return _mapper.Map<DesireDto>(entity);
        }

        public async Task<bool> DeleteDesireAsync(int id)
        {
            return await _desireRepo.DeleteDesireAsync(id);
        }

        public async Task<JqueryDataTablesPagedResults<DesireDto>> GetDesiresDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<Desire, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }


            var desires = (await _desireRepo.GetAllDesiresAsync(filter)).AsQueryable();
            desires = SearchOptionsProcessor<DesireDto, Desire>.Apply(desires, table.Columns);
            desires = SortOptionsProcessor<DesireDto, Desire>.Apply(desires, table);


            var totalRecords = desires.Count();
            var filteredRecords = await _desireRepo.GetCountAsync(filter);

            var data = desires
                .Skip(table.Start)
                .Take(table.Length)
                .Select(d => _mapper.Map<DesireDto>(d))
                .ToList();

            return new JqueryDataTablesPagedResults<DesireDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }


        public async Task<DesireDto?> GetDesireByIdAsync(int id)
        {
            var entity = await _desireRepo.GetDesireByIdAsync(id);
            return entity == null ? null : _mapper.Map<DesireDto>(entity);
        }

        public async Task<DesireDto?> UpdateDesireAsync(int id, DesireDto DesireDto)
        {
            var entity = await _desireRepo.GetDesireForUpdateAsync(id);
            if (entity == null) return null;

            _mapper.Map(DesireDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _desireRepo.UpdateDesireAsync(entity);
            return _mapper.Map<DesireDto>(entity);
        }

        public async Task<IEnumerable<DesireDto>> GetAllDesiresAsync()
        {
            var desires = await _desireRepo.GetAllDesiresAsync();
            return _mapper.Map<IEnumerable<DesireDto>>(desires);

        }
    }
}

