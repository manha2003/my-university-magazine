using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services.UserService;
using BusinessLogicLayer.Services.TokenService;
using DataAccessLayer.Models;

namespace PresentationLayer.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;


        public AuthController(IUserService userService, ILogger<AuthController> logger, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

       

        [HttpPost("login")]
        public async Task<ActionResult> Login (UserDto loginUserDto)
        {
            
           var isAuthenticated = await _userService.LoginAsync(loginUserDto);
           if (!isAuthenticated)
           {
                    return Unauthorized("Login failed. Please check your username and password.");
           }

                
                var token = await _tokenService.CreateToken(loginUserDto);
                return Ok(token);
            
            
        }
        
    }
}
