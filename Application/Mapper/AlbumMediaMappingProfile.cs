using Application.DTOS;
using AutoMapper;
using Data;
using Domain.Models;

namespace Application.Mapper
{
    public class AlbumMediaMappingProfile : Profile
    {
        public AlbumMediaMappingProfile()
        {
            // Domain → DTO
            CreateMap<AlbumMedia, AlbumMediaDto>()
                .ForMember(dest => dest.AlbumName,
                    opt => opt.MapFrom(src => src.Album != null ? src.Album.EnName : string.Empty))
                .ForMember(dest => dest.Picture,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.Picture) ? null : Config.BaseURL + src.Picture))
                .ForMember(dest => dest.YouTubeLink,
                    opt => opt.MapFrom(src => src.YouTubeLink)); 


            CreateMap<AlbumMediaDto, AlbumMedia>()
                .ForMember(dest => dest.Picture,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.Picture) ? null : src.Picture.Replace(Config.BaseURL, "")))
                .ForMember(dest => dest.YouTubeLink,
                    opt => opt.MapFrom(src => src.YouTubeLink));
        }
    }
}
