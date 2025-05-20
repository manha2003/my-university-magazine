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
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentName { get; set; }
        public int ContributionId { get; set; }
        public virtual Contribution Contribution { get; set; } 
        public int UserId { get; set; } 
        public virtual User User { get; set; } 
        public string Content { get; set; } 
        public DateTime CommentDate { get; set; }  //DataTime
    }
}

