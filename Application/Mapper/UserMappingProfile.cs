using Application.DTOS;
using AutoMapper;
using Domain.Models;


namespace Application.Mapper;

public class UserMappingProfile:Profile
{
    public UserMappingProfile()
    {
            
        CreateMap<LoginDto, ApplicationUser>().ReverseMap();
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)).ReverseMap();

    }

   
}
