using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Exception;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.AcademicTermService;
using BusinessLogicLayer.Services.ContributionService;
using BusinessLogicLayer.Services.UserService;
using DataAccessLayer.Repositories.FileRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PresentationLayer.Service;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributionsController : ControllerBase
    {
        private readonly IContributionService _contributionService;
        private readonly IAcademicTermService _academicTermService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IFileRepository _fileRepository;


        public ContributionsController(IContributionService contributionService, IEmailService emailService, IUserService userService, IAcademicTermService academicTermService, IFileRepository fileRepository)
        {
            _contributionService = contributionService;
            _emailService = emailService;
            _userService = userService;
            _academicTermService = academicTermService;
            _fileRepository = fileRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ContributionDto contributionDto)
        {
            try
            {
                // Perform the upload operation
                await _contributionService.UploadContributionAsync(contributionDto);
                /*var marketingCoordinator = await _userService.GetMarketingCoordinatorByFacultyNameAsync(contributionDto.FacultyName);
                var mailRequest = new MailRequest
                {
                    ToEmail = marketingCoordinator.Email,
                    Subject = "New Contribution Submitted From Student " + contributionDto.UserName,
                    Body = GetHtmlcontent()
                };

                await _emailService.SendEmailAsync(mailRequest, contributionDto);*/
                return Ok("Contribution uploaded successfully");

                
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
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


        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmailForContributionUpload([FromForm] int contributionId)
        {
            var contribution = await _contributionService.GetContributionByContributionIdAsync(contributionId);
            var marketingCoordinator = await _userService.GetMarketingCoordinatorByFacultyNameAsync(contribution.FacultyName);
            var mailRequest = new MailRequest
            {
                ToEmail = marketingCoordinator.Email,
                Subject = "New Contribution Submitted From Student " + contribution.UserName,
                Body = GetHtmlcontent()
            };

            await _emailService.SendEmailAsync(mailRequest, contribution);
            return Ok("Email sent successfully!");
        }

        [HttpGet("contribution/{title}")]
        public async Task<IActionResult> GetContributionByTitle(string title)
        {
            try
            {
                var contribution = await _contributionService.GetContributionByContributionNameAsync(title);

                if (contribution == null)
                {
                    return NotFound("No contribution found with the specified title.");
                }

                return Ok(contribution);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("get-all-contributions")]
        public async Task<ActionResult<ContributionDetailsDto>> GetAllContributions()
        {
            var contributions = await _contributionService.GetAllContributionsAsync();
            return Ok(contributions);
        }

        [HttpGet("get-all-contributions-with-academicterm/{academicTermId}")]
        public async Task<ActionResult<ContributionDetailsDto>> GetAllContributionsWithAcademicTerm(int academicTermId)
        {
            var contributions = await _contributionService.GetAllContributionsWithAcademicTermAsync(academicTermId);
            return Ok(contributions);

        }

        [HttpGet("get-all-selected-contributions")]
        public async Task<ActionResult<ContributionDetailsDto>> GetAllSelectedContributions()
        {
            var contributions = await _contributionService.GetAllPublicationContributionsAsync();
            return Ok(contributions);
        }

        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string title)
        {
            var contribution = await _contributionService.GetContributionByContributionNameAsync(title);

            if (contribution == null)
            {
                return NotFound("No contribution found with the specified title.");

            }

            var filename = contribution.FileName;
            var isWordDocument = Path.GetExtension(filename).Equals(".docx", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(filename).Equals(".doc", StringComparison.OrdinalIgnoreCase);

            string contentType;
            byte[] fileBytes;

            if (isWordDocument)
            {
                // Assuming ConvertWordToPdfAsync returns the name of the PDF file
                var pdfFileName = await _fileRepository.ConvertWordToPdfAsync(filename);
                var pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", pdfFileName);
                fileBytes = await System.IO.File.ReadAllBytesAsync(pdfFilePath);
                contentType = "application/pdf";
                filename = pdfFileName; // Return the PDF file name to the client
            }
            else
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filepath, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                fileBytes = await System.IO.File.ReadAllBytesAsync(filepath);
            }

            return File(fileBytes, contentType, Path.GetFileName(filename));
        }


        [HttpGet]
        [Route("DownloadImageFile")]
        public async Task<IActionResult> DownloadImageFile(string title)
        {
            var contribution = await _contributionService.GetContributionByContributionNameAsync(title);
            if (contribution == null)
            {
                return NotFound("No contribution found with the specified title.");

            }
            var imagename = contribution.ImageName;
            var imagepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", imagename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(imagepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(imagepath);
            return File(bytes, contenttype, Path.GetFileName(imagepath));
        }


        [HttpGet]
        [Route("download-all-selected")]
        public async Task<IActionResult> DownloadAllPublicationContributions()
        {
            var recentTerm = await _academicTermService.GetMostRecentlyEndedTermAsync();
            if (recentTerm == null)
            {
                return NotFound("No recently ended academic terms found.");
            }

            var selectedContributions = await _contributionService.GetPublicationContributionsByTermAsync(recentTerm.AcademicTermId);
            if (selectedContributions == null || !selectedContributions.Any())
            {
                return NotFound($"No publications found for the recently ended term: {recentTerm.AcademicYear}.");
            }

            var files = new Dictionary<string, byte[]>();
            foreach (var contribution in selectedContributions)
            {
                if (!string.IsNullOrEmpty(contribution.FileName))
                {
                    var documentBytes = await _fileRepository.GetFileAsync(contribution.FileName);
                    if (documentBytes != null)
                    {
                        files.TryAdd(contribution.FileName, documentBytes);
                    }
                }
                if (!string.IsNullOrEmpty(contribution.ImageName))
                {
                    var imageBytes = await _fileRepository.GetFileAsync(contribution.ImageName);
                    if (imageBytes != null)
                    {
                        files.TryAdd(contribution.ImageName, imageBytes);
                    }
                }
            }

            if (files.Count > 0)
            {
                var zipBytes = ZipUtility.CreateZipFromFiles(files);
                return File(zipBytes, "application/zip", $"Publications_{recentTerm.AcademicYear}.zip");
            }
            else
            {
                return NotFound("No files available to download for the selected contributions.");
            }
        }


        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetContributionsByUsername(string username)
        {
            try
            {
                var contributions = await _contributionService.GetContributionsByUserNameAsync(username);
                if (contributions == null || contributions.Count() == 0)
                {
                    return NotFound($"No contributions found for user {username}.");
                }
                return Ok(contributions);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }


        }

        [HttpGet("faculty/{facultyname}")]
        public async Task<IActionResult> GetContributionsByFacultyname(string facultyname)
        {
            try
            {
                var contributions = await _contributionService.GetContributionsByFacultyNameAsync(facultyname);
                if (contributions == null || contributions.Count() == 0)
                {
                    return NotFound($"No contributions found for faculty {facultyname}.");
                }
                return Ok(contributions);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        [HttpPost("change-contribution-status")]
        public async Task<ActionResult> ChangeContributionStatus([FromForm] int contributionId, string status)
        {
            try
            {
                await _contributionService.ChangeContributionStatusAsync(contributionId, status);
                return Ok($"Contribution: {contributionId} has been successfully updated.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateContribution([FromForm] UpdateContributionDto contributionUpdateDto)
        {
            try
            {
                await _contributionService.UpdateContributionAsync(contributionUpdateDto);
                return Ok("Contribution updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /*[HttpPost]
        [Route("download-selected-contributions")]
        public async Task<IActionResult> DownloadSelectedContributions([FromBody] List<string> selectedContributionTitles)
        {
            var files = new Dictionary<string, byte[]>();

            foreach (var title in selectedContributionTitles)
            {
                var contribution = await _contributionService.GetContributionByContributionNameAsync(title);
                if (contribution != null)
                {
                    
                    if (!string.IsNullOrEmpty(contribution.FileName))
                    {
                        var documentBytes = await _fileRepository.GetFileAsync(contribution.FileName);
                        files.Add(contribution.FileName, documentBytes);
                    }
                    if (!string.IsNullOrEmpty(contribution.ImageName))
                    {
                        var imageBytes = await _fileRepository.GetFileAsync(contribution.ImageName);
                        files.Add(contribution.ImageName, imageBytes);
                    }
                }
            }

            if (files.Count > 0)
            {
                var zipBytes = ZipUtility.CreateZipFromFiles(files);
                return File(zipBytes, "application/zip", "SelectedContributions.zip");
            }
            else
            {
                return NotFound("No files found for the selected contributions.");
            }
        }*/


        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to Greenwich Online University Magazine</h1>";
            Response += "<img src=\"https://centaur-wp.s3.eu-central-1.amazonaws.com/designweek/prod/content/uploads/2016/12/01185323/University-of-Greenwich-logo.jpeg\" />";
            Response += "<h2>You need to comment or provide student some guides about this contribution</h2>";
     
            Response += "</div>";
            return Response;
        }


    }
}

