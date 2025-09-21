using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
{
    public class CityMappingProfile:Profile
    {
        public CityMappingProfile()
        {
            CreateMap<CityDto,CityViewModel>().ReverseMap();
            CreateMap<CountryDto, CityDataTable>().ReverseMap();

        }
    }
}
