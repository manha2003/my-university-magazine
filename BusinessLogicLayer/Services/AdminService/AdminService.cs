using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Repositories.UserRepository;
using DataAccessLayer.Repositories.FacultyRepository;
using Microsoft.Extensions.Logging;
using System;
using BusinessLogicLayer.Validator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Exception;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IRoleValidator _roleValidator;
        public readonly IUserValidator _userValidator;

        private string errorMessage;

        public AdminService(IUserRepository userRepository, IMapper mapper, IRoleValidator roleValidator, IUserValidator userValidator, IFacultyRepository facultyRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _facultyRepository = facultyRepository;
            _userValidator = userValidator;
            _roleValidator = roleValidator;
            _passwordHasher = passwordHasher;
        }

       /* public async Task AssignRoleToUserAsync(RoleAssignmentDto roleAssignmentDto)
        {
            if (!_roleValidator.ValidateRole(roleAssignmentDto.RoleName, out errorMessage))
            {
                throw new InvalidRoleException($"Role '{roleAssignmentDto.RoleName}' is not valid.");
            }

            var user = await _userRepository.GetByUserNameAsync(roleAssignmentDto.UserName);
            if (user == null)
            {
                throw new UserNotFoundException($"User '{roleAssignmentDto.UserName}' not found.");
            }



            await _userRepository.AssignRoleToUserAsync(roleAssignmentDto.UserName, roleAssignmentDto.RoleName);
        }
*/
        public async Task AddNewFacultyAsync(FacultyCreationDto facultyDto)
        {
            var faculty = await _facultyRepository.GetByFacultyNameAsync(facultyDto.FacultyName);
            if (faculty != null)
            {
                throw new FacultyExistedException($"Faculty '{facultyDto.FacultyName}' existed.");
            }

            var facultyEntities = _mapper.Map<Faculty>(facultyDto);
            await _facultyRepository.AddFacultyAsync(facultyEntities);


        }

        public async Task AssignUserToFacultyAsync(string userName, string facultyName)
        {
            // First, validate the user's role
            bool isStudent = await _roleValidator.IsStudentRole(userName);
            bool isMarketingCoordinator = await _roleValidator.IsMarketingCoodinatorRole(userName);
            bool isGuest = await _roleValidator.IsGuestRole(userName);
            if (!(isStudent || isMarketingCoordinator || isGuest))
            {
                throw new InvalidOperationException($"The user '{userName}' is not available.");
            }

            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new KeyNotFoundException($"UserName '{userName}' not found.");

            }

            var faculty = await _facultyRepository.GetByFacultyNameAsync(facultyName);
            if (faculty == null)
            {
                throw new KeyNotFoundException($"Faculty '{facultyName}' not found.");
            }

            if (user.RoleName == "Marketing Coordinator" && await _facultyRepository.HasMarketingCoordinatorAsync(faculty.FacultyId))
            {
                throw new InvalidOperationException($"The faculty '{facultyName}' already has a Marketing Coordinator.");
            }

            if (user.RoleName == "Guest" && await _facultyRepository.HasGuestAsync(faculty.FacultyId))
            {
                throw new InvalidOperationException($"The faculty '{facultyName}' already has a Guest Account.");
            }

            user.FacultyId = faculty.FacultyId;

            
            await _userRepository.UpdateUserAsync(user);

        }


        public async Task AddNewUserAsync(UserAddDto userDto)
        {
            var user = await _userRepository.GetByUserNameAsync(userDto.UserName);
            if (user != null)
            {
                throw new UserExistedException($"User '{userDto.UserName}' existed.");
            }
             
            if (!_roleValidator.ValidateRole(userDto.RoleName, out errorMessage))
            {
                throw new InvalidRoleException($"Role '{userDto.RoleName}' is not valid.");
            }

            if(!_userValidator.ValidateEmail(userDto.Email, out errorMessage))
            {
                throw new EmailException(errorMessage);
            }

            


            var hashedPassword = _passwordHasher.HashPassword(userDto.Password);



            var userEntity = _mapper.Map<User>(userDto);
            userEntity.PasswordHash = hashedPassword;
            await _userRepository.AddUserAsync(userEntity);

        }


        public async Task AddNewGuestAsync(GuestAddDto guestDto)
        {
            var user = await _userRepository.GetByUserNameAsync(guestDto.UserName);
            if (user != null)
            {
                throw new UserExistedException($"User '{guestDto.UserName}' existed.");
            }


            
            var hashedPassword = _passwordHasher.HashPassword(guestDto.Password);



            var userEntity = _mapper.Map<User>(guestDto);
            userEntity.PasswordHash = hashedPassword;
            userEntity.RoleName = "Guest";
            userEntity.Email = GenerateRandomEmail();
            userEntity.FirstName = GenerateRandomFirstName();
            userEntity.LastName = GenerateRandomLastName();

            await _userRepository.AddUserAsync(userEntity);

        }


        private string GenerateRandomEmail()
        {
            return $"guest{Guid.NewGuid()}@example.com";
        }

        private string GenerateRandomFirstName()
        {
            var firstNames = new List<string> { "Alice", "Bob", "Charlie", "David", "Eve" };
            return firstNames[new Random().Next(firstNames.Count)];
        }

        private string GenerateRandomLastName()
        {
            var lastNames = new List<string> { "Smith", "Johnson", "Williams", "Jones", "Brown" };
            return lastNames[new Random().Next(lastNames.Count)];
        }

    }
}

