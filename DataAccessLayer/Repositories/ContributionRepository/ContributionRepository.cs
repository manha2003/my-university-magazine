using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.ContributionRepository
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly OnlineUniversityMagazineDbContext _context;

        public ContributionRepository(OnlineUniversityMagazineDbContext context)
        {
            _context = context;
        }

        public async Task<Contribution> GetContributionByContributionNameAsync(string contributionName)
        {
            return await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(f => f.Title == contributionName);
        }

        public async Task<Contribution> GetByContributionIdAsync(int contributionId)
        {
            return await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(c => c.ContributionId == contributionId);
        }

        public async Task<List<Contribution>> GetPublicationContributionsByTermAsync(int termId)
        {
            return await _context.Contributions
                            .Where(c => c.AcademicTermId == termId && c.Status == "publication") 
                            .ToListAsync();
        }

        public async Task<List<Contribution>> GetAllAsync()
        {
            return await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Faculty).ToListAsync();
        }

        public async Task<List<Contribution>> GetAllSelectedAsync()
        {
            return await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Faculty)
                .Where(c => c.Status == "Selected")
                .ToListAsync();
        }

        public async Task<List<Contribution>> GetContributionsWithAcademicTermAsync(int academicTermId)
        {
            return await _context.Contributions
                .Include(c => c.User)
                .Include(c => c.Faculty)
                .Include(c => c.AcademicTerm)
                .Where(c => c.AcademicTerm.AcademicTermId == academicTermId)
                .ToListAsync();
        }

        public async Task AddAsync(Contribution contribution)
        {
            _context.Contributions.Add(contribution);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Contribution>> GetContributionsByUsernameAsync(string userName)
        {
            
            return await _context.Contributions
                                 .Include(c => c.User) 
                                 .Where(c => c.User.UserName == userName) 
                                 .ToListAsync();
        }

        public async Task<List<Contribution>> GetContributionsByFacultyNameAsync(string facultyName)
        {

            return await _context.Contributions
                                 .Include(c => c.User)
                                 .Include(c => c.Faculty)
                                 .Where(c => c.Faculty.FacultyName== facultyName)
                                 .ToListAsync();
        }



        public async Task UpdateContributionAsync(Contribution contribution)
        {
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();
        }


    }
}
