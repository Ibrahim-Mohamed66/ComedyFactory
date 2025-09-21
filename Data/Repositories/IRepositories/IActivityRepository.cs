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
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetAllActivitiesAsync(
          Expression<Func<Activity, bool>>? filter = null,
          Func<IQueryable<Activity>, IOrderedQueryable<Activity>>? orderBy = null,
          Func<IQueryable<Activity>, IIncludableQueryable<Activity, object>>? include = null
      );

        Task<Activity?> GetActivityByIdAsync(int id);
        Task<bool> AddActivityAsync(Activity activity);
        Task<bool> UpdateActivityAsync(Activity activity);
        Task<bool> DeleteActivityAsync(int id);
        Task<int> GetCountAsync(Expression<Func<Activity, bool>>? filter = null);
        Task<Activity?> GetActivityForUpdateAsync(int id);
    }
}
