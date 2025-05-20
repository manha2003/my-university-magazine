using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validator
{
    public interface IRoleValidator
    {
        public bool ValidateRole(string role, out string errorMessage);
        Task<bool> IsStudentRole(string userName);
        Task<bool> IsMarketingCoodinatorRole(string userName);
        Task<bool> IsGuestRole(string userName);
    }
}
