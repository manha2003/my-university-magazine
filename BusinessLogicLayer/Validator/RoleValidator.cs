using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using DataAccessLayer.Repositories.UserRepository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Services.UserService;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Validator
{
    public class RoleValidator: IRoleValidator
    {
        private readonly IUserRepository _userRepository;

        public RoleValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
        }



        public bool ValidateRole(string role, out string errorMessage)
        {
            List<string> validRoles = new List<string> { "Admin", "Student", "Marketing Coordinator", "Marketing Manager", "Guest" };
            errorMessage = null;
            if (!validRoles.Contains(role))
            {
                errorMessage = "Invalid Role.";
                return false;
            }


            return true;
        }

        public async Task <bool> IsStudentRole(string userName)
        {
           
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new KeyNotFoundException($"User '{userName}' not found.");
            }

            return user.RoleName.Equals("Student", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> IsMarketingCoodinatorRole(string userName)
        {

            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new KeyNotFoundException($"User '{userName}' not found.");
            }

            return user.RoleName.Equals("Marketing Coordinator", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> IsGuestRole(string userName)
        {

            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new KeyNotFoundException($"User '{userName}' not found.");
            }

            return user.RoleName.Equals("Guest", StringComparison.OrdinalIgnoreCase);
        }


    }
}

