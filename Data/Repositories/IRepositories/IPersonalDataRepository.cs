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
    public interface IPersonalDataRepository
    {
        Task<IEnumerable<PersonalData>> GetAllPersonalDatasAsync(
            Expression<Func<PersonalData, bool>>? filter = null,
            Func<IQueryable<PersonalData>, IOrderedQueryable<PersonalData>>? orderBy = null,
            Func<IQueryable<PersonalData>, IIncludableQueryable<PersonalData, object>>? include = null
        );

        Task<PersonalData?> GetPersonalDataByIdAsync(int id);
        Task<bool> AddPersonalDataAsync(PersonalData PersonalData);
        Task<int> GetCountAsync(Expression<Func<PersonalData, bool>>? filter = null);
        Task<PersonalData?> GetPersonalDataForUpdateAsync(int id);
    }
}
