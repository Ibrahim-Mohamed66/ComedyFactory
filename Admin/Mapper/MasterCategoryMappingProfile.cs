using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
{
    public class MasterCategoryMappingProfile:Profile
    {
        public MasterCategoryMappingProfile()
        {
            CreateMap<MasterCategoryDto,MasterCategoryViewModel>().ReverseMap();
            CreateMap<MasterCategoryDto, MasterCategoryDataTable>().ReverseMap();
        }
    }
}
