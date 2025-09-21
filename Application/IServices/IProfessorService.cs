using Application.DTOS;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;

namespace Application.IServices
{
    public interface IProfessorService
    {
        Task<JqueryDataTablesPagedResults<ProfessorDto>> GetProfessorsDataTableAsync(JqueryDataTablesParameters table);
        Task<ProfessorDto?> GetProfessorByIdAsync(int id);
        Task<ProfessorDto> CreateProfessorAsync(ProfessorDto professorDto);
        Task<ProfessorDto?> UpdateProfessorAsync(int id, ProfessorDto professorDto);
        Task<bool> DeleteProfessorAsync(int id);
        Task<IEnumerable<ProfessorDto>> GetAllProfessorsAsync();
    }
}
