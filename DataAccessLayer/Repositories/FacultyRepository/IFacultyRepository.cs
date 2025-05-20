using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.FacultyRepository
{
    public interface IFacultyRepository
    {

        Task<List<Faculty>> GetAllFacultiesAsync();
        Task<Faculty> GetByFacultyNameAsync(string facultyName);
        Task UpdateFacultyAsync(Faculty faculty);
        Task AddFacultyAsync(Faculty faculty);
        Task DeleteAsync(string facultyName);
        Task<bool> HasUsersAsync(int facultyId);
        Task<bool> HasMarketingCoordinatorAsync(int facultyId);
        Task<bool> HasGuestAsync(int facultyId);


    }
}
