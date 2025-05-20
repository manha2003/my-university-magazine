using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Data;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string RoleName { get; set; }
        public int? FacultyId { get; set; }
        
        
        public Faculty? Faculty { get; set; }
        
        public ICollection<Contribution>? Contributions { get; set; }
        
    }
}
