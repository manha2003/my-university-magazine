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
    public class UserMapperProfiles : Profile
    {
        public UserMapperProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<User, UserFacultyAssignmentDto>()
            .ForMember(dto => dto.UserName, conf => conf.MapFrom(usr => usr.UserName))
        
            .ForMember(dto => dto.FacultyName, conf => conf.MapFrom(usr => usr.Faculty != null ? usr.Faculty.FacultyName : "No Faculty"));

            CreateMap<User, UserAddDto>().ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<User, GuestAddDto>().ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
               ;

            CreateMap<User, RoleAssignmentDto>().ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));
            CreateMap<User, UserDetailsDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))

                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.FacultyName));



        }
    }
}
