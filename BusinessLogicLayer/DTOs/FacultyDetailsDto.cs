using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class FacultyDetailsDto
    {
        public string FacultyName { get; set; }
        public List<RoleAssignmentDto> Users { get; set; }
    }
}
