using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories.IRepositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System.Linq.Expressions;


namespace Techvally.Application.Service
{
    public class MasterCategoryService : IMasterCategoryService
    {
        private readonly IMasterCategoryRepository _masterCategoryRepo;
        private readonly IMapper _mapper;

        public MasterCategoryService(IMasterCategoryRepository masterCategoryRepo, IMapper mapper)
        {
            _masterCategoryRepo = masterCategoryRepo;
            _mapper = mapper;
        }

        public async Task<MasterCategoryDto> CreateMasterCategoryAsync(MasterCategoryDto MasterCategoryDto)
        {
            var entity = _mapper.Map<MasterCategory>(MasterCategoryDto);

            var result = await _masterCategoryRepo.AddMasterCategoryAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create MasterCategory");
            }
            return _mapper.Map<MasterCategoryDto>(entity);
        }

        public async Task<bool> DeleteMasterCategoryAsync(int id)
        {
            return await _masterCategoryRepo.DeleteMasterCategoryAsync(id);
        }

        public async Task<JqueryDataTablesPagedResults<MasterCategoryDto>> GetMasterCategoriesDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<MasterCategory, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }


            var MasterCategorys = (await _masterCategoryRepo.GetAllMasterCategoriesAsync(filter)).AsQueryable();
            MasterCategorys = SearchOptionsProcessor<MasterCategoryDto, MasterCategory>.Apply(MasterCategorys, table.Columns);
            MasterCategorys = SortOptionsProcessor<MasterCategoryDto, MasterCategory>.Apply(MasterCategorys, table);


            var totalRecords = MasterCategorys.Count();
            var filteredRecords = await _masterCategoryRepo.GetCountAsync(filter);

            var data = MasterCategorys
                .Skip(table.Start)
                .Take(table.Length)
                .Select(d => _mapper.Map<MasterCategoryDto>(d))
                .ToList();

            return new JqueryDataTablesPagedResults<MasterCategoryDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }


        public async Task<MasterCategoryDto?> GetMasterCategoryByIdAsync(int id)
        {
            var entity = await _masterCategoryRepo.GetMasterCategoryByIdAsync(id);
            return entity == null ? null : _mapper.Map<MasterCategoryDto>(entity);
        }

        public async Task<MasterCategoryDto?> UpdateMasterCategoryAsync(int id, MasterCategoryDto MasterCategoryDto)
        {
            var entity = await _masterCategoryRepo.GetMasterCategoryForUpdateAsync(id);
            if (entity == null) return null;

            _mapper.Map(MasterCategoryDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _masterCategoryRepo.UpdateMasterCategoryAsync(entity);
            return _mapper.Map<MasterCategoryDto>(entity);
        }

        public async Task<IEnumerable<MasterCategoryDto>> GetAllMasterCategoriesAsync()
        {
            var MasterCategorys = await _masterCategoryRepo.GetAllMasterCategoriesAsync();
            return _mapper.Map<IEnumerable<MasterCategoryDto>>(MasterCategorys);

        }
    }
}
