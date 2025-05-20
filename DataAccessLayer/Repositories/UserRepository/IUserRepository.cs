using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.UserRepository
{
    public interface IUserRepository
    {

        Task<List<User>> GetAllUserAsync();
        Task<List<User>> GetAllStudentsWithFacultyAsync();
        Task<User> GetByUserNameAsync(string userName);
        Task UpdateUserAsync(User user);
        Task AddUserAsync(User user);
        Task DeleteAsync(string userName);
        Task AssignRoleToUserAsync(string userName, string roleName);
        Task<User> GetMarketingCoordinatorByFacultyNameAsync(string facultyName);
        public bool IsEmailUnique(string email);
    }
}
