using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IMasterCategoryService
    {
        Task<JqueryDataTablesPagedResults<MasterCategoryDto>> GetMasterCategoriesDataTableAsync(JqueryDataTablesParameters table);
        Task<MasterCategoryDto?> GetMasterCategoryByIdAsync(int id);
        Task<MasterCategoryDto> CreateMasterCategoryAsync(MasterCategoryDto masterCategoryDto);
        Task<MasterCategoryDto?> UpdateMasterCategoryAsync(int id, MasterCategoryDto masterCategoryDto);
        Task<bool> DeleteMasterCategoryAsync(int id);
        Task<IEnumerable<MasterCategoryDto>> GetAllMasterCategoriesAsync();
    }
}
