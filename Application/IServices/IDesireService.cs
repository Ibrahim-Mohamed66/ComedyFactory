

using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IDesireService
    {
        Task<JqueryDataTablesPagedResults<DesireDto>> GetDesiresDataTableAsync(JqueryDataTablesParameters table);
        Task<DesireDto?> GetDesireByIdAsync(int id);
        Task<DesireDto> CreateDesireAsync(DesireDto desireDto);
        Task<DesireDto?> UpdateDesireAsync(int id, DesireDto desireDto);
        Task<bool> DeleteDesireAsync(int id);
        Task<IEnumerable<DesireDto>> GetAllDesiresAsync();
    }
}
