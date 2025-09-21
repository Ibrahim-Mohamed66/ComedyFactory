using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;
using Data;

namespace Admin.Mapper
{
    public class ActivityMappingProfile : Profile
    {
        public ActivityMappingProfile()
        {
            CreateMap<ActivityDto,ActivityViewModel>()
                .ForMember(dest => dest.Icon,
                opt => opt.MapFrom(src =>
                    src.Icon != null ? Config.BaseURL + src.Icon : null)).ReverseMap();
            CreateMap<ActivityDataTable, ActivityDto>().ReverseMap();
        }
    }
}
