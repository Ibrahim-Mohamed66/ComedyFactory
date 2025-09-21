using Application.DTOS;
using AutoMapper;
using Data;
using Domain.Models;

namespace Application.Mapper
{
    public class ActivityMappingProfile : Profile
    {
        public ActivityMappingProfile()
        {
            // Entity → DTO
            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon != null ? Config.BaseURL + src.Icon : null));
            CreateMap<ActivityDto, Activity>();
        }
    }
}