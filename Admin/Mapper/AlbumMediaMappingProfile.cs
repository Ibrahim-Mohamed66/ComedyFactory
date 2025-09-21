using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;
using Domain.Models;

namespace Admin.Mapper
{
    public class AlbumMediaMappingProfile:Profile
    {
        public AlbumMediaMappingProfile()
        {
            CreateMap<AlbumMediaDto,AlbumMediaViewModel>().ReverseMap();
            CreateMap<AlbumMediaDto, AlbumMediaDataTable>().ReverseMap();
        }
    }
}
