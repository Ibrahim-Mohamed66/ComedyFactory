using Application.DTOS;
using AutoMapper;
using Data;
using Domain.Models;

namespace Application.Mapper
{
    public class BlockMappingProfile : Profile
    {
        public BlockMappingProfile()
        {
            // Domain → DTO
            CreateMap<Block, BlockDto>()
                .ForMember(dest => dest.ArPicture,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ArPicture) ? null : Config.BaseURL + src.ArPicture))
                .ForMember(dest => dest.EnPicture,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.EnPicture) ? null : Config.BaseURL + src.EnPicture));

            // DTO → Domain (strip BaseURL)
            CreateMap<BlockDto, Block>()
                .ForMember(dest => dest.ArPicture,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ArPicture) ? null : src.ArPicture.Replace(Config.BaseURL, "")))
                .ForMember(dest => dest.EnPicture,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.EnPicture) ? null : src.EnPicture.Replace(Config.BaseURL, "")));
        }
    }
}
