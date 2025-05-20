using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.FacultyService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultiesController(IFacultyService facultyService)
        {
           _facultyService = facultyService;
        }

        [HttpGet("get-all-faculties")]
        public async Task<ActionResult<FacultyCreationDto>> GetAllFaculties()
        {
            var faculties = await _facultyService.GetAllFacultiesAsync();
            return Ok(faculties);
        }


        [HttpGet("{facultyName}")]
        public async Task<ActionResult<FacultyDetailsDto>> GetFacultyFacultyName(string facultyName)
        {
            var faculty = await _facultyService.GetFacultyByFacultyNameAsync(facultyName);

            if (faculty == null)
            {
                return StatusCode(404, " No faculty found");
            }

            return Ok(faculty);
        }

        

    }
}
