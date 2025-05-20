using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        
    }
}
