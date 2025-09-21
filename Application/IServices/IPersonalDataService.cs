using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;

namespace Application.IServices
{
    public interface IPersonalDataService
    {
        Task<JqueryDataTablesPagedResults<PersonalDataDto>> GetPersonDatasDataTableAsync(JqueryDataTablesParameters table);
        Task<PersonalDataDto?> GetPersonDataByIdAsync(int id);
        Task<PersonalDataDto> CreatePersonDataAsync(PersonalDataDto personalDataDto);
    }
}
