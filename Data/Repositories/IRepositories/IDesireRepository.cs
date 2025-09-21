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
    public interface IDesireRepository
    {
        Task<IEnumerable<Desire>> GetAllDesiresAsync(
          Expression<Func<Desire, bool>>? filter = null,
          Func<IQueryable<Desire>, IOrderedQueryable<Desire>>? orderBy = null,
          Func<IQueryable<Desire>, IIncludableQueryable<Desire, object>>? include = null
      );

        Task<Desire?> GetDesireByIdAsync(int id);
        Task<bool> AddDesireAsync(Desire Desire);
        Task<bool> UpdateDesireAsync(Desire Desire);
        Task<bool> DeleteDesireAsync(int id);
        Task<int> GetCountAsync(Expression<Func<Desire, bool>>? filter = null);
        Task<Desire?> GetDesireForUpdateAsync(int id);
    }
}
