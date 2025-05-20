using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.UserService;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using DataAccessLayer.Repositories;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Repositories.UserRepository;

namespace BusinessLogicLayer.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public TokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<string> CreateToken(UserDto userDto)
        {
            var user =  await _userRepository.GetByUserNameAsync(userDto.UserName);
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, userDto.UserName),

                new Claim(ClaimTypes.Role, user.RoleName),

            };

            
                
            



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
