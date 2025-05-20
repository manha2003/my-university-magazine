using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class UserAddDto
    {
        public string UserName { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
       
    };                                                                               
}
