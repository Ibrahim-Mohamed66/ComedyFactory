using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper
{
    public class ConfigurationMappingProfile:Profile
    {
        public ConfigurationMappingProfile()
        {
            CreateMap<ConfigurationDto, ConfigurationViewModel>().ReverseMap();
        }
    }
}
