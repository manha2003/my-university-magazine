using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class UserContributionDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool TermsAgreed { get; set; }

        public string SubmissionDate { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }

    }
}
