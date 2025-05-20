using BusinessLogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.AdminService
{
    public interface IAdminService
    {
       // Task AssignRoleToUserAsync(RoleAssignmentDto roleAssignmentDto);
        Task AddNewFacultyAsync(FacultyCreationDto facultyDto);
        Task AssignUserToFacultyAsync(string userName, string facultyName);
        Task AddNewGuestAsync(GuestAddDto guestDto);
        Task AddNewUserAsync(UserAddDto userDto);
    }
}
