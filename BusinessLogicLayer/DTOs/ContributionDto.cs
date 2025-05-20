using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class ContributionDto
    {
     //   public string Title { get; set; }
   //     public string Description { get; set; }
        public string UserName { get; set; } 
        public string FacultyName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        
        public bool TermsAgreed { get; set; }
        
        public IFormFile ImageFile { get; set; }
        public IFormFile DocumentFile { get; set; }




    }
}
