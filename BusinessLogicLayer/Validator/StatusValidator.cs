using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validator
{
    public class StatusValidator
    {
        public async Task<bool> ValidateStatusAsync(string status)
        {
            List<string> validStatuses = new List<string> { "Refer", "Publication", };
            
            if (!validStatuses.Contains(status))
            {
              
                return false;
            }


            return true;
        }
    }
}
