using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validator
{
    public interface IUserValidator
    {
        public bool ValidateEmail(string email , out string errorMessage);
    }
}
