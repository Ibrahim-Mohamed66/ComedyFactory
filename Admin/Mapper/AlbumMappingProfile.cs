using Admin.DataTable;
using Admin.ViewModels;
using Application.DTOS;
using AutoMapper;

namespace Admin.Mapper;

public class AlbumMappingProfile : Profile
{
    public AlbumMappingProfile() 
    {
        CreateMap<AlbumViewModel, AlbumDto>().ReverseMap();
        CreateMap<AlbumDto, AlbumDataTable>().ReverseMap();
    }
}
