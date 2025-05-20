using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services.AcademicTermService;
using BusinessLogicLayer.Exception;
using System.Data;


namespace PresentationLayer.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]

        public class AcademicTermsController : ControllerBase
        {
            private readonly IAcademicTermService _academicTermService;

            public AcademicTermsController(IAcademicTermService academicTermService)
            {
                _academicTermService = academicTermService;
            }

        [HttpPost("add-new-academic-term")]
            public async Task<IActionResult> CreateAcademicTerm(AcademicTermDto academicTermDto)
            {
                try
                {

                    await _academicTermService.AddAcademicTermAsync(academicTermDto);
                    return Ok("Academic term created successfully.");
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (FormatException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                 {
                      
                   return StatusCode(500, "An unexpected error occurred.");
                 }
        }

        [HttpGet("{id}")]
            public async Task<IActionResult> GetAcademicTerm(int id)
            {
                var academicTerm = await _academicTermService.GetAcademicTermByIdAsync(id);
                if (academicTerm == null)
                {
                    return NotFound("Academic term not found.");
                }
                return Ok(academicTerm);
            }

         [HttpGet]
            public async Task<IActionResult> GetAllAcademicTerms()
            {
                var academicTerms = await _academicTermService.GetAllAcademicTermsAsync();
                return Ok(academicTerms);
            }

            /*[HttpPut("{id}")]
            public async Task<IActionResult> UpdateAcademicTerm(int id, AcademicTermDto academicTermDto)
            {
                var result = await _academicTermService.UpdateAcademicTermAsync(id, academicTermDto);
                if (!result) return NotFound("Academic term not found for update.");
                return Ok("Academic term updated successfully.");
            }*/
        }
    }

