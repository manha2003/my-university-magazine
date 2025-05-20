using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Models;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.FacultyRepository
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly OnlineUniversityMagazineDbContext _context;

        public FacultyRepository(OnlineUniversityMagazineDbContext context)
        {
            _context = context;
        }

        public async Task<List<Faculty>> GetAllFacultiesAsync()
        {
            return await _context.Faculties.ToListAsync();
        }

        public async Task<Faculty> GetByFacultyNameAsync(string facultyName)
        {
            return await _context.Faculties
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => f.FacultyName == facultyName);
        }


        public async Task AddFacultyAsync(Faculty faculty)
        {
            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string facultyName)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.FacultyName == facultyName);

            if (faculty == null)
            {
                throw new ArgumentException("Faculty not found.");
            }

            // Check if there are users associated with the faculty by ID
            bool hasUsers = await HasUsersAsync(faculty.FacultyId);

            if (hasUsers)
            {
                throw new InvalidOperationException("Faculty has associated users. Cannot delete.");
            }

            // Delete the faculty
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateFacultyAsync(Faculty faculty)
        {
            _context.Faculties.Update(faculty);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUsersAsync(int facultyId)
        {
            
            return await _context.Users.AnyAsync(u => u.FacultyId == facultyId);
        }

        public async Task<bool> HasMarketingCoordinatorAsync(int facultyId)
        {
           
            return await _context.Users.AnyAsync(u => u.FacultyId == facultyId && u.RoleName == "Marketing Coordinator");
        }

        public async Task<bool> HasGuestAsync(int facultyId)
        {
           
            return await _context.Users.AnyAsync(u => u.FacultyId == facultyId && u.RoleName == "Guest");
        }
    }

}
