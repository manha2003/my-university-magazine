using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Validator
{
    public class AcademicTermValidator : IAcademicTermValidator
    { 
        public bool ValidateAcademicYear(string academicYear, out string errorMessage)
        {
        
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{4}-\d{4}$");
            if (!regex.IsMatch(academicYear))
            {
                errorMessage = "Academic year must be in the format YYYY-YYYY.";
                return false;
            }

            var years = academicYear.Split('-');
            if (int.Parse(years[1]) - int.Parse(years[0]) != 1)
            {
                errorMessage = "Academic year range should be consecutive years.";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
