using BusinessLogicLayer.DTOs;
using System.Collections.Generic;
using BusinessLogicLayer.DTOs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.UserService
{
    public interface IUserService
    {
        Task<bool> LoginAsync(UserDto loginUserDto);
        Task<List<UserDetailsDto>> GetAllUsersAsync();
        Task<List<UserFacultyAssignmentDto>> GetAllStudentsWithFacultyAsync();
        Task<UserDetailsDto> GetUserByUserNameAsync(string userName);
        Task<UserDetailsDto> GetMarketingCoordinatorByFacultyNameAsync(string facultyName);
        Task UpdateUserAsync(UpdateUserDto userDto);
        Task DeleteUserAsync(string userName);
        Task<bool> IsUserMarketingCoordinatorInFacultyAsync(string userName, string facultyName);
    }
}
