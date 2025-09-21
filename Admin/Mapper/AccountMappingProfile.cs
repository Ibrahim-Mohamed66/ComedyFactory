
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
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
