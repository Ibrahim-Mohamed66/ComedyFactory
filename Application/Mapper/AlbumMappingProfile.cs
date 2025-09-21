using Application.DTOS;
using AutoMapper;
using Data;
using Domain.Models;


namespace Application.Mapper
{
    public class AlbumMappingProfile:Profile
    {
        public AlbumMappingProfile()
        {
            CreateMap<Album,AlbumDto>()
                 .ForMember(dest => dest.ArPicture,
                opt => opt.MapFrom(src =>
                    src.ArPicture != null ? Config.BaseURL + src.ArPicture : null))
                .ForMember(dest => dest.EnPicture,
                opt => opt.MapFrom(src =>
                    src.EnPicture != null ? Config.BaseURL + src.EnPicture : null));
            // DTO → Domain (strip BaseURL, save only relative path)
            CreateMap<AlbumDto, Album>()
                .ForMember(dest => dest.ArPicture,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.ArPicture) ? null : src.ArPicture.Replace(Config.BaseURL, "")))
                .ForMember(dest => dest.EnPicture,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.EnPicture) ? null : src.EnPicture.Replace(Config.BaseURL, "")));
        }
    }
}
