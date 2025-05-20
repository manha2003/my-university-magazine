using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.DTOs;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.CommentService
{
    public interface ICommentService
    {
        Task AddNewCommentAsync(CommentAddDto commentDto);

        Task<List<CommentDetailsDto>> GetCommentsByContributionNameAsync(string contributionName);
        Task<List<CommentDetailsDto>> GetCommentsByContributionIdAsync(int contributionId);
        Task DeleteCommentAsync(string commentName);
    }
}
