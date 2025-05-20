using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class CommentDetailsDto
    {
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string CommentName { get; set; }
        public string ContributionId { get; set; }
        public DateTime CommentDate { get; set; }
        public string Content { get; set; }
    }
}
