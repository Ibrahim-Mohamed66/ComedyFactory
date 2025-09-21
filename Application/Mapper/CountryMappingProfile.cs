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
    public class CountryMappingProfile:Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<CountryDto, Country>()
                        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Country, CountryDto>();
        }

    }
}
