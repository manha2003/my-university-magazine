using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class CommentAddDto
    {
        public string CommentName { get; set; }
        public int ContributionId { get; set; }
        public string ContributionName { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
       
    
    }
}
