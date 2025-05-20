using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogicLayer.DTOs;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.FacultyService
{
    public interface IFacultyService
    {
        Task<List<FacultyCreationDto>> GetAllFacultiesAsync();
        Task<FacultyDetailsDto> GetFacultyByFacultyNameAsync(string facultyName);
        Task DeleteFacultyAsync(string facultyName);
    }
}
