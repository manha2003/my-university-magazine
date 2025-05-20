using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Repositories.UserRepository;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validator
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;
        private string errorMessage;

        public UserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public bool ValidateEmail(string email, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(email))
            {
                errorMessage = "Email is Required";
                return false;
            }
            if (!email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Email must be in the format '@gmail.com'";
                return false;
            }
            if (_userRepository.IsEmailUnique(email))
            {
                errorMessage = "This Email has been taken";
                return false;
            }
            return true;
        }

       

    }
}
