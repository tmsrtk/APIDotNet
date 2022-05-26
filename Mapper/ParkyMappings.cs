using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDotNet.Entities;
using AutoMapper;

namespace APIDotNet.Mapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}