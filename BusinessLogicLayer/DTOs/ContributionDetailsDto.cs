using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class ContributionDetailsDto
    {
        public int ContributionId {  get; set; }
        public string UserName { get; set; }
        public string FacultyName { get; set; }
        public string Title { get; set; }
        public int AcademicTermId { get; set; }
        public string Description { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public string ImageName { get; set; }

        
       

    }
}
