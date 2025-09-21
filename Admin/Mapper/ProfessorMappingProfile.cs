using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
{
    public class ProfessorMappingProfile:Profile
    {
        public ProfessorMappingProfile()
        {
            CreateMap<ProfessorDto,ProfessorViewModel>().ReverseMap();
            CreateMap<ProfessorDataTable,ProfessorDto>().ReverseMap();
           
            
        }
    }

}
