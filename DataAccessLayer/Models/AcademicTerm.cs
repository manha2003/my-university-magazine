using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Data;

namespace DataAccessLayer.Models
{
    public class AcademicTerm
    {
        public int AcademicTermId { get; set; } 
        public string AcademicYear { get; set; }
        
        public DateTime EntryDate { get; set; }
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosure { get; set; }

        
        public ICollection<Contribution>? Contributions { get; set; }
    }
}
