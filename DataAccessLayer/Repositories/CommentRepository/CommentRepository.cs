using System;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly OnlineUniversityMagazineDbContext _context;

        public CommentRepository(OnlineUniversityMagazineDbContext context)
        {
            _context = context;
        }
        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsByContributionNameAsync(string contributionName)
        {
            return await _context.Comments
               .Include(c => c.Contribution)
               .Where(c => c.Contribution.Title == contributionName)
               .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByContributionIdAsync(int contributionId)
        {
            return await _context.Comments
               .Include(c => c.Contribution)
               .Include(c => c.User)
               .Where(c => c.Contribution.ContributionId == contributionId)
               .ToListAsync();
        }

        public async Task<Comment> GetByCommentNameAsync(string commentName)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.CommentName == commentName);
        }


        public async Task DeleteAsync(Comment comment)
        {
            
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            
        }


    }
}
