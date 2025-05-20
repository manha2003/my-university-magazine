using BusinessLogicLayer.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Services.TokenService
{
    public interface ITokenService
    {
       Task <string> CreateToken(UserDto userDto);
    }
}
