using DataAccessLayer.Models;
using DataAccessLayer.Repositories.UserRepository;
using System;
using AutoMapper;
using BusinessLogicLayer.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Helpers;
using Microsoft.Extensions.Logging;
using BusinessLogicLayer.Exception;
using DataAccessLayer.Repositories.FacultyRepository;

namespace BusinessLogicLayer.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IPasswordHasher _passwordHasher;  
        
        private readonly IMapper _mapper;
      

        public UserService(IUserRepository userRepository, IFacultyRepository facultyRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
          
        }

        

        public async Task<bool> LoginAsync(UserDto loginUserDto)

        {
            
            var user = await _userRepository.GetByUserNameAsync(loginUserDto.UserName);
            if (user == null)
            {
                return false;

            }

            var result = _passwordHasher.VerifyPassword(loginUserDto.Password, user.PasswordHash);
            if (!result)
            {
                return false;
               
            }


            return result;
           
        }

        public async Task<List<UserDetailsDto>> GetAllUsersAsync()
        {
            var userEntities = await _userRepository.GetAllUserAsync();
            return _mapper.Map<List<UserDetailsDto>>(userEntities); 
        }

        public async Task<List<UserFacultyAssignmentDto>> GetAllStudentsWithFacultyAsync()
        {
            var userEntities = await _userRepository.GetAllStudentsWithFacultyAsync();
            return _mapper.Map<List<UserFacultyAssignmentDto>>(userEntities);
        }



        public async Task<UserDetailsDto> GetUserByUserNameAsync(string userName)
        {

            var userEntity = await _userRepository.GetByUserNameAsync(userName);
            
            return _mapper.Map<UserDetailsDto>(userEntity);

        }


        public async Task DeleteUserAsync(string userName)
        {
            await _userRepository.DeleteAsync(userName);
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto)
        {
            var user = await _userRepository.GetByUserNameAsync(userDto.UserName);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with user name {userDto.UserName} not found.");
            }

            var hashedPassword = _passwordHasher.HashPassword(userDto.Password);

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PasswordHash = hashedPassword;
            await _userRepository.UpdateUserAsync(user);

        }

        private async Task AssignDefaultRoleToUser(User user)
        {
            var defaultRoleName = "Default";
            user.RoleName = defaultRoleName;
           
            await _userRepository.UpdateUserAsync(user);
        }


        public async Task<UserDetailsDto> GetMarketingCoordinatorByFacultyNameAsync(string facultyName)
        {
            var userEntity = await _userRepository.GetMarketingCoordinatorByFacultyNameAsync(facultyName);
            return _mapper.Map<UserDetailsDto>(userEntity);

        }

        public async Task<bool> IsUserMarketingCoordinatorInFacultyAsync(string userName, string facultyName)
        {
            
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return false; 
            
            var faculty = await _facultyRepository.GetByFacultyNameAsync(facultyName);
            if (faculty == null) return false; 

           
            return user.FacultyId == faculty.FacultyId && user.RoleName == "Marketing Coordinator";
        }

       
    }
}
