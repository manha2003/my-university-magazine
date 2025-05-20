using System;
using AutoMapper;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.DTOs;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapping
{
    public class FacultyMapperProfiles : Profile
    {
        public FacultyMapperProfiles()
        {
            CreateMap<Faculty, FacultyCreationDto>().ReverseMap()
               .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.FacultyName));
            CreateMap<Faculty, FacultyDetailsDto>().ReverseMap()
               .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.FacultyName))
               .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));

            

        }
    }
}
