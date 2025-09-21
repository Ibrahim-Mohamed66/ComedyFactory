using Application.DTOS;
using AutoMapper;
using Domain.Models;
using System.Globalization;

namespace Application.Mapper
{
    public class PersonalDataMappingProfile : Profile
    {
        public PersonalDataMappingProfile()
        {
            CreateMap<PersonalData, PersonalDataDto>()
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src =>
                        src.City == null
                            ? null
                            : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar"
                                ? src.City.ArName
                                : src.City.EnName
                    ))
                .ForMember(dest => dest.Country,
                    opt => opt.MapFrom(src =>
                        src.Country == null
                            ? null
                            : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar"
                                ? src.Country.ArName
                                : src.Country.EnName
                    ))
                .ForMember(dest => dest.Desire,
                    opt => opt.MapFrom(src =>
                        src.Desire == null
                            ? null
                            : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar"
                                ? src.Desire.ArName
                                : src.Desire.EnName
                    ))
                .ReverseMap();
        }
    }
}
