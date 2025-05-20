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
    public class Faculty
    {
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        

       
        public virtual ICollection<User>? Users { get; set; } 
        public virtual ICollection<Contribution>? Contributions { get; set; } 

    }
}
