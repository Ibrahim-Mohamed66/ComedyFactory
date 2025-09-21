using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;
using Domain.Models;

namespace Admin.Mapper
{
    public class CountryMappingProfile:Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<CountryDto, CountryViewModel>().ReverseMap();
            CreateMap<CountryDto, CountryDataTable>().ReverseMap();
        }
    }
}
