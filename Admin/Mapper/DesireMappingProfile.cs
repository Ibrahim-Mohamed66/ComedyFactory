using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
{
    public class DesireMappingProfile:Profile
    {
        public DesireMappingProfile()
        {
            CreateMap<DesireDto,DesireViewModel>().ReverseMap();
            CreateMap<DesireDataTable,DesireDto>().ReverseMap();
        }
    }

}
