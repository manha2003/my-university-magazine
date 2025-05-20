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
    public class ContributionMapperProfiles : Profile
    {
        public ContributionMapperProfiles()
        {
            CreateMap<ContributionDto, Contribution>()
       
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.FacultyId, opt => opt.Ignore())
            .ForMember(dest => dest.ContributionId, opt => opt.Ignore())
            .ForMember(dest => dest.ImageName, opt => opt.Ignore())
            .ForMember(dest => dest.FileName, opt => opt.Ignore())
            .ForMember(dest => dest.FileType, opt => opt.Ignore())
           
            ;

            // Optionally, if you want to map from Contribution back to ContributionDto
            CreateMap<Contribution, ContributionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)) // Assuming User entity has a UserName
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.FacultyName)) // Assuming Faculty entity has a Name
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentFile, opt => opt.Ignore()) // File is not stored in Contribution entity
                ;
            /* CreateMap<Contribution, ContributionDetailsDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User..UserName)) // Assuming User entity has a UserName
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.FacultyName)) // Assuming Faculty entity has a Name
                ;
             CreateMap<Contribution, ContributionDetailsDto>().ReverseMap()

                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
               ;*/

            CreateMap<Contribution, ContributionDetailsDto>()
            .ForMember(dest => dest.ContributionId, opt => opt.MapFrom(src => src.ContributionId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.FacultyName))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.AcademicTermId, opt => opt.MapFrom(src => src.AcademicTermId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName))
            // Reverse mapping configuration
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Faculty, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName));
            

             CreateMap<Contribution, UserContributionDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));

            CreateMap<UpdateContributionDto, Contribution>()
            .ForMember(dest => dest.ContributionId, opt => opt.MapFrom(src => src.ContributionId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ImageName, opt => opt.Ignore())
            .ForMember(dest => dest.FileName, opt => opt.Ignore())
            .ForMember(dest => dest.FileType, opt => opt.Ignore());

            CreateMap<Contribution, UpdateContributionDto>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentFile, opt => opt.Ignore());

        }
    }
}
