using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;
using Data;


namespace Admin.Mapper
{
    public class BlockMappingProfile:Profile
    {
        public BlockMappingProfile()
        {
            CreateMap<BlockViewModel,BlockDto>()
                .ForMember(dest => dest.ArPicture,
                opt => opt.MapFrom(src =>
                    src.ArPicture != null ? Config.BaseURL + src.ArPicture : null))
                .ForMember(dest => dest.EnPicture,
                opt => opt.MapFrom(src =>
                    src.EnPicture != null ? Config.BaseURL + src.EnPicture : null)).ReverseMap();
        }
    }
}
