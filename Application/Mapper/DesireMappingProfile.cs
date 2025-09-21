using Application.DTOS;
using AutoMapper;
using Domain.Models;

namespace Application.Mapper
{
    public class DesireMappingProfile:Profile
    {
        public DesireMappingProfile()
        {
            
            CreateMap<Desire, DesireDto>().ReverseMap();
        }
                       
    }
}
