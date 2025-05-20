using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class AcademicTermDto
    {
       
        public string AcademicYear { get; set; }
        public string EntryDate { get; set; }
        public string ClosureDate { get; set; }
        public string FinalClosure { get; set; }
    }
}
