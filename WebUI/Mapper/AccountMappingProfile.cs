using Application.DTOS;
using AutoMapper;
using WebUI.Models;
using WebUI.ViewModels;

namespace WebUI.Mapper
{
    public class AccountMappingProfile:Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<LoginDto, LoginVm>().ReverseMap();
            CreateMap<RegisterDto, RegisterVm>().ReverseMap();
        }
    }
}
