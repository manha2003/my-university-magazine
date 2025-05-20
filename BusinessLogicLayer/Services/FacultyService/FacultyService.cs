using System;
using System.Collections.Generic;
using BusinessLogicLayer.DTOs; 

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories.FacultyRepository;
using DataAccessLayer.Repositories.UserRepository;
using AutoMapper;

namespace BusinessLogicLayer.Services.FacultyService
{
    public class FacultyService : IFacultyService

    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;

        public FacultyService(IFacultyRepository facultyRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
        }

        
        public async Task<List<FacultyCreationDto>> GetAllFacultiesAsync()
        {
            var facultyEntities = await _facultyRepository.GetAllFacultiesAsync();
            return _mapper.Map<List<FacultyCreationDto>>(facultyEntities);
        }
        public async Task<FacultyDetailsDto> GetFacultyByFacultyNameAsync(string facultyName)
        {
            var facultyEntity = await _facultyRepository.GetByFacultyNameAsync(facultyName);
            return _mapper.Map<FacultyDetailsDto>(facultyEntity);

        }

        public async Task DeleteFacultyAsync(string facultyName)
        {
           
            

            await _facultyRepository.DeleteAsync(facultyName);
        }
    }
}
