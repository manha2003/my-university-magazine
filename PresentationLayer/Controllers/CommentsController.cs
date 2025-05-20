using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Exception;
using BusinessLogicLayer.Services.CommentService;
using BusinessLogicLayer.Helpers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.Internal;
using BusinessLogicLayer.Services.UserService;
using BusinessLogicLayer.Services.ContributionService;
using PresentationLayer.Service;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IContributionService _contributionService;
        private readonly IUserService _userService;
        private readonly ICommentEmailService _commentEmailService;


        public CommentsController(ICommentService commentService, IContributionService contributionService,IUserService userService, ICommentEmailService commentEmailService)
        {
            _commentService = commentService;
            _contributionService = contributionService;
            _userService = userService;
            _commentEmailService = commentEmailService;
        }

        [HttpPost("add-new-comment")]
        public async Task<IActionResult> AddNewComment([FromForm] CommentAddDto commentDto)
        {
            try
            {
                
                await _commentService.AddNewCommentAsync(commentDto);
                var contribution = await _contributionService.GetContributionByContributionNameAsync(commentDto.ContributionName);
                var user = await _userService.GetUserByUserNameAsync(contribution.UserName);
                var userComment = await _userService.GetUserByUserNameAsync(commentDto.UserName);
                if (userComment.RoleName == "Marketing Coordinator") 
                {
                    var mailRequest = new MailRequest
                    {
                        ToEmail = user.Email,
                        Subject = "New Comment for your contribution From Marketing Coordinator ",
                        Body = GetHtmlcontent()
                    };
                    await _commentEmailService.SendEmailAsync(mailRequest);
                }
                
                
                return Ok("Comment Added successfully");
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }
            
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("get-comment-list/{contributionId}")]
        public async Task<ActionResult<CommentDetailsDto>> GetCommentsByContributionId(int contributionId)
        {
            try
            {
                var contributions = await _commentService.GetCommentsByContributionIdAsync(contributionId);

                return Ok(contributions);
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("delete-comment/{commentName}")]

        public async Task<ActionResult> DeleteComment(string commentName)
        {
            try
            {

                await _commentService.DeleteCommentAsync(commentName);
                return new NoContentResult();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred.");
            }

        }

        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to Greenwich Online University Magazine</h1>";
            Response += "<img src=\"https://centaur-wp.s3.eu-central-1.amazonaws.com/designweek/prod/content/uploads/2016/12/01185323/University-of-Greenwich-logo.jpeg\" />";
            Response += "<h2>Marketing Coordinator have commented your contribution</h2>";
            Response += "</div>";
            return Response;
        }


    }
}
