using System;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLayer.Repositories.AcademicTermRepository
{
    public interface IAcademicTermRepository
    {
        Task AddAcademicTermAsync(AcademicTerm academicTerm);
        Task<AcademicTerm> GetMostRecentlyEndedTermAsync();
        Task<AcademicTerm> GetAcademicTermByIdAsync(int academicTermId);
        Task<AcademicTerm> GetCurrentTermAsync(DateTime currentDate);
        Task<List<AcademicTerm>> GetAllAcademicTermsAsync();
       // Task UpdateAcademicTermAsync(AcademicTerm academicTerm);

    }
}
