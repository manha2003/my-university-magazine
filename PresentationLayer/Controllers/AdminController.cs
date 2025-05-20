using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Authorization;

using BusinessLogicLayer.Services.AdminService;
using BusinessLogicLayer.Exception;
using BusinessLogicLayer.Services.UserService;
using System.Data;
using PresentationLayer.Service;
using BusinessLogicLayer.Services.FacultyService;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;

namespace PresentationLayer.Controllers
{
    [ApiController]
    //, Authorize(Roles = "Admin")

    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;
        private readonly IFacultyService _facultyService;
        private readonly IUserCreatedEmailService _userCreatedEmailService;

        public AdminController(IAdminService adminService, IUserService userService, IFacultyService facultyService, IUserCreatedEmailService userCreatedEmailService)
        {
            _adminService = adminService;
            _userService = userService;
            _facultyService = facultyService;
            _userCreatedEmailService = userCreatedEmailService;
        }

        /*  [HttpPost("assign-role")]
          public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentDto roleAssignment)
          {

              try
              {
                  await _adminService.AssignRoleToUserAsync(roleAssignment);
                  return Ok("Role assigned successfully.");
              }
              catch (InvalidRoleException ex)
              {
                  return BadRequest(ex.Message);
              }
              catch (UserNotFoundException ex)
              {
                  return BadRequest(ex.Message);
              }
              catch (Exception ex)
              {
                  // Log the exception and return a generic error response
                  return StatusCode(500, "An unexpected error occurred.");
              }
          }
  */
        [HttpPost("add-new-faculty")]
        public async Task<ActionResult<FacultyCreationDto>> AddNewFaculty(FacultyCreationDto facultyDto)
        {
            try
            {
                await _adminService.AddNewFacultyAsync(facultyDto);
                return Ok($"Faculty {facultyDto.FacultyName} added successfully");
            }

            catch (FacultyExistedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                return StatusCode(500, "An unexpected error occurred.");
            }

        }





        [HttpPost("assign-user-to-faculty")]
        public async Task<ActionResult> AssignUserToFaculty([FromBody] UserFacultyAssignmentDto assignmentDto)
        {
            try
            {
                await _adminService.AssignUserToFacultyAsync(assignmentDto.UserName, assignmentDto.FacultyName);
                return Ok($"User {assignmentDto.UserName} has been successfully assigned to the faculty: {assignmentDto.FacultyName}.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add-new-user")]
        public async Task<ActionResult> AddNewUser([FromBody] UserAddDto userDto)
        {
            try
            {
                userDto.Password = GenerateRandomPassword();
                await _adminService.AddNewUserAsync(userDto);
                var mailRequest = new MailRequest
                {
                    ToEmail = userDto.Email,
                    Subject = "Account for your Greenwich University Magazine Website",
                    Body = GetHtmlcontent(userDto.UserName, userDto.Password)
                };
                await _userCreatedEmailService.SendEmailAsync(mailRequest);
                
                return Ok($"User {userDto.UserName} added successfully");
            }
            catch (InvalidRoleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserExistedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmailException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
               
                return StatusCode(500, "An unexpected error occurred.");
            }


        }


        [HttpPost("add-new-guest")]
        public async Task<ActionResult> AddNewGuest([FromBody] GuestAddDto guestDto)
        {
            try
            {

                await _adminService.AddNewGuestAsync(guestDto);

                return Ok($"Guest {guestDto.UserName} added successfully");
            }
            catch (InvalidRoleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserExistedException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred.");
            }


        }


        [HttpDelete("delete-user")]

        public async Task<ActionResult> DeleteUser(string userName)
        {
            var existingUser = await _userService.GetUserByUserNameAsync(userName);

            if (existingUser == null)
            {
                return StatusCode(404, "No User found");
            }

            await _userService.DeleteUserAsync(userName);
            return new NoContentResult();

        }

        [HttpDelete("delete-faculty")]

        public async Task<ActionResult> DeleteFaculty(string facultyName)
        {
            var existingFaculty = await _facultyService.GetFacultyByFacultyNameAsync(facultyName);

            if (existingFaculty == null)
            {
                return StatusCode(404, "No faculty found");
            }

            await _facultyService.DeleteFacultyAsync(facultyName);
            return new NoContentResult();

        }


        private string GenerateRandomPassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 8) 
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private string GetHtmlcontent(string username, string password)
        {
            string response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            response += "<h1>Welcome to Greenwich Online University Magazine</h1>";
            response += "<img src=\"https://www.sableinternational.com/images/default-source/blog/university-of-greenwich-blog.jpg?sfvrsn=2ef53e0_1\" />";
            response += "<h2>Your Account:</h2>";
            response += $"<p>Username: {username}</p>";
            response += $"<p>Password: {password}</p>";
            response += "</div>";
            return response;
        }
    }
}
