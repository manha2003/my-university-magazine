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
    public class AcademicTermRepository : IAcademicTermRepository
    {
        private readonly OnlineUniversityMagazineDbContext _context;

        public AcademicTermRepository(OnlineUniversityMagazineDbContext context)
        {
            _context = context;
        }

        public async Task AddAcademicTermAsync(AcademicTerm academicTerm)
        {
            var mostRecentTerm = await _context.AcademicTerms
                                       .OrderByDescending(t => t.FinalClosure)
                                       .FirstOrDefaultAsync();

            if (mostRecentTerm != null && mostRecentTerm.FinalClosure >= academicTerm.EntryDate)
            {
                throw new InvalidOperationException("A new academic term can only be added after the last academic term has ended.");
            }
            _context.AcademicTerms.Add(academicTerm);
            await _context.SaveChangesAsync();
        }

        public async Task<AcademicTerm> GetMostRecentlyEndedTermAsync()
        {
            return await _context.AcademicTerms
                            .Where(t => t.FinalClosure <= DateTime.UtcNow) 
                            .OrderByDescending(t => t.FinalClosure)
                            .FirstOrDefaultAsync();
        }


        public async Task<AcademicTerm> GetAcademicTermByIdAsync(int academicTermId)
        {
            return await _context.AcademicTerms.FirstOrDefaultAsync(at => at.AcademicTermId == academicTermId);
        }

        public async Task<List<AcademicTerm>> GetAllAcademicTermsAsync()
        {
            return await _context.AcademicTerms.ToListAsync();
        }

        /*public async Task UpdateAcademicTermAsync(AcademicTerm academicTerm)
        {
            _context.AcademicTerms.Update(academicTerm);
            await _context.SaveChangesAsync();
        }*/

        public async Task<AcademicTerm> GetCurrentTermAsync(DateTime currentDate)
        {
            return await _context.AcademicTerms
                .FirstOrDefaultAsync(t => t.EntryDate <= currentDate && t.ClosureDate >= currentDate);
        }

        public async Task DeleteAcademicTermAsync(AcademicTerm academicTerm)
        {
            _context.AcademicTerms.Remove(academicTerm);
            await _context.SaveChangesAsync();
        }
    }
}
