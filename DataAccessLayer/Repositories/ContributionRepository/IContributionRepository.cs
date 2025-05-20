using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.ContributionRepository
{
    public interface IContributionRepository
    {
        Task AddAsync(Contribution contribution);
        Task<Contribution> GetContributionByContributionNameAsync(string contributionName);
        Task<Contribution> GetByContributionIdAsync(int contributionId);
        Task<List<Contribution>> GetAllAsync();
        Task<List<Contribution>> GetAllSelectedAsync();
        Task<List<Contribution>> GetContributionsByUsernameAsync(string username);
        Task<List<Contribution>> GetContributionsWithAcademicTermAsync(int academicTermId);
        Task<List<Contribution>> GetPublicationContributionsByTermAsync(int termId);
        Task<List<Contribution>> GetContributionsByFacultyNameAsync(string facultyName);
        Task UpdateContributionAsync(Contribution contribution);
    }
}
