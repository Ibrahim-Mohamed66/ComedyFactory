using Application.DTOS;
using AutoMapper;
using WebUI.ViewModels;

namespace WebUI.Mapper
{
    public class PersonalDataMappingProfile :Profile
    {
        public PersonalDataMappingProfile()
        {
            CreateMap<PersonalDataDto, PersonalDataViewModel>().ReverseMap();
        }

    }
}
