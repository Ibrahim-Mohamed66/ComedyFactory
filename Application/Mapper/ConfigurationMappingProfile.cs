using Application.DTOS;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class ConfigurationMappingProfile:Profile
    {
        public ConfigurationMappingProfile()
        {
            CreateMap<Configuration, ConfigurationDto>().ReverseMap();
        }
    }
}
