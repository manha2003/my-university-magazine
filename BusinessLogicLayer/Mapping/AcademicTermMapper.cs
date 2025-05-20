using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapping
{
    public class AcademicTermMapperProfiles : Profile
    {
        public AcademicTermMapperProfiles()
        {
            CreateMap<AcademicTermDto, AcademicTerm>()
            
            .ForMember(dest => dest.EntryDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.EntryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.ClosureDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.ClosureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.FinalClosure, opt => opt.MapFrom(src => DateTime.ParseExact(src.FinalClosure, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

        
            CreateMap<AcademicTerm, AcademicTermDto>()
                
                .ForMember(dest => dest.EntryDate, opt => opt.MapFrom(src => src.EntryDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ClosureDate, opt => opt.MapFrom(src => src.ClosureDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.FinalClosure, opt => opt.MapFrom(src => src.FinalClosure.ToString("yyyy-MM-dd")));


            CreateMap<AcademicTermDetailsDto, AcademicTerm>()
            .ForMember(dest => dest.AcademicTermId, opt => opt.MapFrom(src => src.AcademicTermId))
           .ForMember(dest => dest.EntryDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.EntryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
           .ForMember(dest => dest.ClosureDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.ClosureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
           .ForMember(dest => dest.FinalClosure, opt => opt.MapFrom(src => DateTime.ParseExact(src.FinalClosure, "yyyy-MM-dd", CultureInfo.InvariantCulture)));


            CreateMap<AcademicTerm, AcademicTermDetailsDto>()
                .ForMember(dest => dest.AcademicTermId, opt => opt.MapFrom(src => src.AcademicTermId))
                .ForMember(dest => dest.EntryDate, opt => opt.MapFrom(src => src.EntryDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ClosureDate, opt => opt.MapFrom(src => src.ClosureDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.FinalClosure, opt => opt.MapFrom(src => src.FinalClosure.ToString("yyyy-MM-dd")));
        }
    }
    
}
