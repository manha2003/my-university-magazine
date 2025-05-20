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
    public class Contribution
    {
        public int ContributionId { get; set; } // Primary key
        
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; } 
        public virtual User User { get; set; } 
        public int FacultyId { get; set; } 
        public virtual Faculty Faculty { get; set; } 
        public DateTime SubmissionDate { get; set; }
        public int AcademicTermId { get; set; }
        public virtual AcademicTerm AcademicTerm { get; set; }
       
        public string? Status { get; set; } 
        public bool TermsAgreed { get; set; }
        public string? ImageName { get; set; }
        public string FileName { get; set; } 
        public string FileType { get; set; } 

        
        

        
        public virtual ICollection<Comment>? Comments { get; set; } // Navigation property to comments on the contribution
    }
}
