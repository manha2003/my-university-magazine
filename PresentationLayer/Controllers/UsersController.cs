using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services.UserService;


namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all-user")]
        public async Task<ActionResult<UserDetailsDto>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("get-all-student-with-faculty")]
        public async Task<ActionResult<IEnumerable<UserFacultyAssignmentDto>>> GetStudents()
        {

            var users = await _userService.GetAllStudentsWithFacultyAsync();
            return Ok(users);

        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return StatusCode(404, " No User found");
            }

            return Ok(user);
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto userDto)
        {
            try
            {
                await _userService.UpdateUserAsync(userDto);
                return Ok("User updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
