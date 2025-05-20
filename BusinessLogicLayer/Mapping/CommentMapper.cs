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
    public class CommentMapperProfiles : Profile
    {
        public CommentMapperProfiles()
        {
            CreateMap<CommentAddDto, Comment>()
            .ForMember(dest => dest.CommentId, opt => opt.Ignore()) 
            .ForMember(dest => dest.CommentName, opt => opt.MapFrom(src => src.CommentName))
            .ForMember(dest => dest.ContributionId, opt => opt.MapFrom(src => src.ContributionId)) 
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CommentDate, opt => opt.Ignore()); 

          
            CreateMap<Comment, CommentAddDto>()
                .ForMember(dest => dest.CommentName, opt => opt.MapFrom(src => src.CommentName))
                .ForMember(dest => dest.ContributionName, opt => opt.MapFrom(src => src.Contribution.Title))
                .ForMember(dest => dest.ContributionId, opt => opt.MapFrom(src => src.ContributionId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)) 
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<Comment, CommentDetailsDto>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.CommentId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ContributionId, opt => opt.MapFrom(src => src.Contribution.ContributionId))
                .ForMember(dest => dest.CommentName, opt => opt.MapFrom(src => src.CommentName))
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.CommentDate))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))

                
                
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Contribution, opt => opt.Ignore())
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.CommentId))
                .ForMember(dest => dest.CommentName, opt => opt.MapFrom(src => src.CommentName))
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.CommentDate))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));





        }
    }
}
