using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class UpdateContributionDto
    {
        public int ContributionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public IFormFile ImageFile { get; set; }
        public IFormFile DocumentFile { get; set; }
    }
}
