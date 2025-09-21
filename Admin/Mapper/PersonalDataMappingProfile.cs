using Admin.DataTable;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
{
    public class PersonalDataMappingProfile : Profile
    {
        public PersonalDataMappingProfile()
        {
            CreateMap<PersonalDataDto, PersonalDataTable>()
               .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
               .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
               .ForMember(dest => dest.Genders, opt => opt.MapFrom(src => src.Genders))
               .ForMember(dest => dest.Desire, opt => opt.MapFrom(src => src.Desire));

        }
    }
}
