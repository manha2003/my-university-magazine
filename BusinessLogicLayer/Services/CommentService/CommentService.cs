using System;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.UserService;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories.CommentRepository;
using DataAccessLayer.Repositories.ContributionRepository;
using DataAccessLayer.Repositories.UserRepository;
using AutoMapper;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IContributionRepository _contributionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository _commnetRepository,IContributionRepository contributionRepository, IUserRepository userRepository, IUserService userService, IMapper mapper)
        {
            _commentRepository = _commnetRepository;
            _contributionRepository = contributionRepository;
            _userRepository = userRepository;
            _userService = userService; 
            _mapper = mapper;
        }

        public async Task AddNewCommentAsync(CommentAddDto commentDto)
        {
            
            var user = await _userRepository.GetByUserNameAsync(commentDto.UserName);
            if (user == null) throw new KeyNotFoundException($"User with username {commentDto.UserName} not found.");

            
            var contribution = await _contributionRepository.GetByContributionIdAsync(commentDto.ContributionId);
            if (contribution == null)
            {
                throw new KeyNotFoundException($"Contribution with name {commentDto.ContributionName} not found.");
            }

            var facultyName = contribution.Faculty.FacultyName;

            var marketCoordinatorVerify = await _userService.IsUserMarketingCoordinatorInFacultyAsync(commentDto.UserName, facultyName);
            if (!marketCoordinatorVerify && !(contribution.UserId == user.UserId))
            {
                throw new InvalidOperationException($" User {commentDto.UserName} is not a marketing coordinator in {facultyName} or Student of the contribution");
            }


            // Map CommentAddDto to Comment entity
            var comment = _mapper.Map<Comment>(commentDto);
            
            
            comment.UserId = user.UserId;
            comment.ContributionId = contribution.ContributionId;

           
            comment.CommentDate = DateTime.Now;

        
            await _commentRepository.AddCommentAsync(comment);
        }


        public async Task<List<CommentDetailsDto>> GetCommentsByContributionNameAsync(string contributionName)
        {
            var commentEntities = await _commentRepository.GetCommentsByContributionNameAsync(contributionName);
            if (commentEntities == null)
            {
                throw new InvalidOperationException("No Comment found!");
            }
            return _mapper.Map<List<CommentDetailsDto>>(commentEntities);
        }

        public async Task<List<CommentDetailsDto>> GetCommentsByContributionIdAsync(int contributionId)
        {
            var commentEntities = await _commentRepository.GetCommentsByContributionIdAsync(contributionId);
            if (commentEntities == null)
            {
                throw new InvalidOperationException("No Comment found!");
            }
            return _mapper.Map<List<CommentDetailsDto>>(commentEntities);
        }

        public async Task<CommentAddDto> GetCommentByCommentNameAsync(string commentName)
        {
            var commentEntity = await _commentRepository.GetByCommentNameAsync(commentName);
            return _mapper.Map<CommentAddDto>(commentEntity);

        }



        public async Task DeleteCommentAsync(string commentName)
        {
            var commentEntity = await _commentRepository.GetByCommentNameAsync(commentName);
            if(commentEntity == null)
            {
                throw new KeyNotFoundException("Comment can not be found");
            }
 
            await _commentRepository.DeleteAsync(commentEntity);
        }

    }
}
