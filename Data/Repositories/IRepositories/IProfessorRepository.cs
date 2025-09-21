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
    public interface IProfessorRepository
    {
        Task<IEnumerable<Professor>> GetAllProfessorsAsync(
            Expression<Func<Professor, bool>>? filter = null,
            Func<IQueryable<Professor>, IOrderedQueryable<Professor>>? orderBy = null,
            Func<IQueryable<Professor>, IIncludableQueryable<Professor, object>>? include = null
        );

        Task<Professor?> GetProfessorByIdAsync(int id);
        Task<bool> AddProfessorAsync(Professor Professor);
        Task<bool> UpdateProfessorAsync(Professor Professor);
        Task<bool> DeleteProfessorAsync(int id);
        Task<int> GetCountAsync(Expression<Func<Professor, bool>>? filter = null);
        Task<Professor?> GetProfessorForUpdateAsync(int id);
    }
}
