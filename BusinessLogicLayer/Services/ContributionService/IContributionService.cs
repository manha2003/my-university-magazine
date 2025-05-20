using BusinessLogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.ContributionService
{
    public interface IContributionService
    {
        Task UploadContributionAsync(ContributionDto contributionDto);
        Task<ContributionDetailsDto> GetContributionByContributionNameAsync(string contributionName);
        Task<List<ContributionDetailsDto>> GetAllContributionsAsync();
        Task<List<ContributionDetailsDto>> GetAllPublicationContributionsAsync();
        Task<List<ContributionDetailsDto>> GetPublicationContributionsByTermAsync(int termId);
        Task<List<ContributionDetailsDto>> GetAllContributionsWithAcademicTermAsync(int academicTermId);
        Task<ContributionDetailsDto> GetContributionByContributionIdAsync(int contributionId);
        Task<List<UserContributionDto>> GetContributionsByUserNameAsync(string userName);
        Task<List<UserContributionDto>> GetContributionsByFacultyNameAsync(string facultyName);
        Task ChangeContributionStatusAsync(int contributionId, string status);
        Task UpdateContributionAsync(UpdateContributionDto contributionUpdateDto);
    }
}
