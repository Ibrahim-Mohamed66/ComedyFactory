using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IActivityService
    {
        Task<JqueryDataTablesPagedResults<ActivityDto>> GetActivitiesDataTableAsync(JqueryDataTablesParameters table);
        Task<ActivityDto?> GetActivityByIdAsync(int id);
        Task<ActivityDto> CreateActivityAsync(ActivityDto ActivityDto);
        Task<ActivityDto?> UpdateActivityAsync(int id, ActivityDto ActivityDto);
        Task<bool> DeleteActivityAsync(int id);
        Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync();
    }
}
