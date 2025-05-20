using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Data;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.UserRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly OnlineUniversityMagazineDbContext _context;

        public UserRepository(OnlineUniversityMagazineDbContext context)
        {
            _context = context;
        }

      

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _context.Users
            .Include(u => u.Faculty)
            .Where(u => u.RoleName.Equals("Student") || u.RoleName.Equals("Marketing Coordinator")|| u.RoleName.Equals("Marketing Manager") || u.RoleName.Equals("Guest"))
            .ToListAsync();
        }

        public async Task<List<User>> GetAllStudentsWithFacultyAsync()
        {
            return await _context.Users
           .Include(u => u.Faculty) 
           .Where(u => u.RoleName.Equals("Student"))
           .ToListAsync();
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.Faculty)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }


        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userName)
        {
            var user = await GetByUserNameAsync(userName);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AssignRoleToUserAsync(string userName, string roleName)
        {

            var user = await GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.RoleName = roleName;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetMarketingCoordinatorByFacultyNameAsync(string facultyName)
        {
            return await _context.Users
                .Include(u => u.Faculty)
                .Where(u => u.Faculty.FacultyName == facultyName && u.RoleName == "Marketing Coordinator")
                .FirstOrDefaultAsync();
        }

        public bool IsEmailUnique(string email)
        {

            return _context.Users.Any(u => u.Email == email) ;
        }

    }
}
