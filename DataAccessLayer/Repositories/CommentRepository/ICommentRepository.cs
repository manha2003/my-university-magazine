using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Models;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.CommentRepository
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsByContributionNameAsync(string contributionName);
        Task<List<Comment>> GetCommentsByContributionIdAsync(int contributionId);
        Task <Comment> GetByCommentNameAsync(string commentName);
        Task DeleteAsync(Comment comment);
    }
}
