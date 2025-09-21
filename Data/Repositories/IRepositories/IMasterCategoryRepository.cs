using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.IRepositories
{
    public interface IMasterCategoryRepository
    {
        Task<IEnumerable<MasterCategory>> GetAllMasterCategoriesAsync(
            Expression<Func<MasterCategory, bool>>? filter = null,
            Func<IQueryable<MasterCategory>, IOrderedQueryable<MasterCategory>>? orderBy = null,
            Func<IQueryable<MasterCategory>, IIncludableQueryable<MasterCategory, object>>? include = null
        );

        Task<MasterCategory?> GetMasterCategoryByIdAsync(int id);
        Task<bool> AddMasterCategoryAsync(MasterCategory MasterCategory);
        Task<bool> UpdateMasterCategoryAsync(MasterCategory MasterCategory);
        Task<bool> DeleteMasterCategoryAsync(int id);
        Task<int> GetCountAsync(Expression<Func<MasterCategory, bool>>? filter = null);
        Task<MasterCategory?> GetMasterCategoryForUpdateAsync(int id);

    }
}
