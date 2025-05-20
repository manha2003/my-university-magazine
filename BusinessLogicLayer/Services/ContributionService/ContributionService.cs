using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Exception;
using BusinessLogicLayer.Validator;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.ContributionRepository;
using DataAccessLayer.Repositories.AcademicTermRepository;
using DataAccessLayer.Repositories.FacultyRepository;
using DataAccessLayer.Repositories.FileRepository;
using DataAccessLayer.Repositories.UserRepository;

namespace BusinessLogicLayer.Services.ContributionService
{
    public class ContributionService : IContributionService
    {
        private readonly IContributionRepository _contributionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IAcademicTermRepository _academicTermRepository;
        private readonly StatusValidator _statusValidator;
        private readonly IMapper _mapper;

        public ContributionService(IContributionRepository contributionRepository, IFileRepository fileRepository, IAcademicTermRepository academicTermRepository, IMapper mapper, IUserRepository userRepository, IFacultyRepository facultyRepository, StatusValidator statusValidator)
        {
            _contributionRepository = contributionRepository;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _facultyRepository = facultyRepository;
            _academicTermRepository = academicTermRepository;
            _statusValidator = statusValidator;
            _mapper = mapper;
        }

        public async Task UploadContributionAsync(ContributionDto contributionDto)
        {

            if (!contributionDto.TermsAgreed)
            {
                throw new InvalidOperationException("Terms must be agreed to.");
            }

            var user = await _userRepository.GetByUserNameAsync(contributionDto.UserName);
            if (user == null)
            {
                throw new UserNotFoundException($"User {contributionDto.UserName} Not Found");
            }
            var faculty = await _facultyRepository.GetByFacultyNameAsync(contributionDto.FacultyName);
            if (faculty == null)
            {
                throw new KeyNotFoundException($"Faculty {contributionDto.FacultyName} Not Found");
            }

            if (user.FacultyId != faculty.FacultyId)
            {
                throw new InvalidOperationException($"User {contributionDto.UserName} is not part of the faculty {contributionDto.FacultyName}.");
            }

            if (contributionDto.ImageFile != null && !contributionDto.ImageFile.ContentType.StartsWith("image/"))
            {
                throw new InvalidOperationException("The ImageFile must be an image.");
            }

            if (contributionDto.DocumentFile != null &&
                !(contributionDto.DocumentFile.ContentType.Equals("application/pdf") ||
                  contributionDto.DocumentFile.ContentType.Equals("application/msword") ||
                  contributionDto.DocumentFile.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document")))
            {
                throw new InvalidOperationException("The DocumentFile must be a document (PDF or Word).");
            }
            var currentAcademicTerm = await _academicTermRepository.GetCurrentTermAsync(DateTime.Now);
            if (currentAcademicTerm == null)
            {
                throw new KeyNotFoundException("No active academic term found.");
            }

            // Proceed with validations knowing you have the current academic term
            if (DateTime.Now < currentAcademicTerm.EntryDate || DateTime.Now > currentAcademicTerm.ClosureDate)
            {
                throw new InvalidOperationException("New contributions can only be uploaded between the entry date and closure date of the current academic term.");
            }

            var imageFilePath = await _fileRepository.SaveFileAsync(contributionDto.ImageFile);
            var documentFilePath = await _fileRepository.SaveFileAsync(contributionDto.DocumentFile);


            var contributionEntities = _mapper.Map<Contribution>(contributionDto);
            contributionEntities.UserId = user.UserId;
            contributionEntities.FacultyId = faculty.FacultyId;
            contributionEntities.AcademicTermId = currentAcademicTerm.AcademicTermId;
            contributionEntities.ImageName = imageFilePath;
            contributionEntities.FileName = documentFilePath;   
            contributionEntities.Status = "Pending";
            contributionEntities.SubmissionDate = DateTime.Now;
            contributionEntities.FileType = contributionDto.DocumentFile.ContentType;

            await _contributionRepository.AddAsync(contributionEntities);

        }

        public async Task<ContributionDetailsDto> GetContributionByContributionNameAsync(string contributionName)
        {
            var contributionEntity = await _contributionRepository.GetContributionByContributionNameAsync(contributionName);
            return _mapper.Map<ContributionDetailsDto>(contributionEntity);
        }

        public async Task<ContributionDetailsDto> GetContributionByContributionIdAsync(int contributionId)
        {
            var contributionEntity = await _contributionRepository.GetByContributionIdAsync(contributionId);
            return _mapper.Map<ContributionDetailsDto>(contributionEntity);
        }


        public async Task<List<ContributionDetailsDto>> GetAllContributionsAsync()
        {
            var contributionEntities = await _contributionRepository.GetAllAsync();
            return _mapper.Map<List<ContributionDetailsDto>>(contributionEntities);
        }

        public async Task<List<ContributionDetailsDto>> GetAllContributionsWithAcademicTermAsync(int academicTermId)
        {
            var academicTerm = await _academicTermRepository.GetAcademicTermByIdAsync(academicTermId);
            if (academicTerm == null)
            {
                throw new KeyNotFoundException("Selected academic term not found.");
            }
            var contributionEntities = await _contributionRepository.GetContributionsWithAcademicTermAsync(academicTermId);
            return _mapper.Map<List<ContributionDetailsDto>>(contributionEntities);
        }

        public async Task<List<ContributionDetailsDto>> GetPublicationContributionsByTermAsync(int termId)
        {
            var contributionEntities = await _contributionRepository.GetPublicationContributionsByTermAsync(termId);
            return _mapper.Map<List<ContributionDetailsDto>>(contributionEntities);
        }

        public async Task<List<ContributionDetailsDto>> GetAllPublicationContributionsAsync()
        {
            var contributionEntities = await _contributionRepository.GetAllSelectedAsync();
            return _mapper.Map<List<ContributionDetailsDto>>(contributionEntities);
        }

        public async Task<List<UserContributionDto>> GetContributionsByUserNameAsync(string userName)
        {
            var contributionEntities = await _contributionRepository.GetContributionsByUsernameAsync(userName);
            return _mapper.Map<List<UserContributionDto>>(contributionEntities);
        }

        public async Task<List<UserContributionDto>> GetContributionsByFacultyNameAsync(string facultyName)
        {
            var contributionEntities = await _contributionRepository.GetContributionsByFacultyNameAsync(facultyName);
            return _mapper.Map<List<UserContributionDto>>(contributionEntities);
        }

        public async Task ChangeContributionStatusAsync(int contributionId, string status)
        {
            var isValid = await _statusValidator.ValidateStatusAsync(status);
            if (!isValid)
            {
                throw new InvalidOperationException("Contribution status is not valid");
            }
            var contributionEntity = await _contributionRepository.GetByContributionIdAsync(contributionId);

            if (contributionEntity == null)
            {
                throw new KeyNotFoundException($"Contribution '{contributionId}' not found.");

            }
            contributionEntity.Status = status;

            await _contributionRepository.UpdateContributionAsync(contributionEntity);

        }

        /* public async Task UpdateContributionAsync(UpdateContributionDto contributionUpdateDto)
         {
             var contribution = await _contributionRepository.GetByContributionIdAsync(contributionUpdateDto.ContributionId);

             if (contribution == null)
             {
                 throw new KeyNotFoundException($"Contribution with ID {contributionUpdateDto.ContributionId} not found.");
             }


             contribution.Title = contributionUpdateDto.Title;
             contribution.Description = contributionUpdateDto.Description;

             await _contributionRepository.UpdateContributionAsync(contribution);

             if (contributionUpdateDto.ImageFile != null && !contributionUpdateDto.ImageFile.ContentType.StartsWith("image/"))
             {
                 throw new InvalidOperationException("The ImageFile must be an image.");
             }

             if (contributionUpdateDto.DocumentFile != null &&
                 !(contributionUpdateDto.DocumentFile.ContentType.Equals("application/pdf") ||
                   contributionUpdateDto.DocumentFile.ContentType.Equals("application/msword") ||
                   contributionUpdateDto.DocumentFile.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document")))
             {
                 throw new InvalidOperationException("The DocumentFile must be a document (PDF or Word).");
             }
             await _fileRepository.DeleteFileAsync(contribution.FileName);
             var newDocumentFilePath = await _fileRepository.SaveFileAsync(contributionUpdateDto.DocumentFile);
             var newImageFilePath = await _fileRepository.SaveFileAsync(contributionUpdateDto.ImageFile);
             var contributionEntity = _mapper.Map<Contribution>(contributionUpdateDto);

             contributionEntity.ImageName = newImageFilePath;
             contributionEntity.FileName = newDocumentFilePath;
             contributionEntity.FileType = contributionUpdateDto.DocumentFile.ContentType;
             await _contributionRepository.UpdateContributionAsync(contributionEntity);
         }*/

        public async Task UpdateContributionAsync(UpdateContributionDto contributionUpdateDto)
        {
            var contribution = await _contributionRepository.GetByContributionIdAsync(contributionUpdateDto.ContributionId);

            if (contribution == null)
            {
                throw new KeyNotFoundException($"Contribution with ID {contributionUpdateDto.ContributionId} not found.");
            }

            var academicTerm = await _academicTermRepository.GetAcademicTermByIdAsync(contribution.AcademicTermId);
            if (academicTerm.ClosureDate > DateTime.Now || academicTerm.FinalClosure < DateTime.Now)
            {
                throw new InvalidOperationException("Contribution can only update after closure date and before final closure date");
            }

            contribution.Title = contributionUpdateDto.Title;
            contribution.Description = contributionUpdateDto.Description;

            
            if (contributionUpdateDto.DocumentFile != null)
            {
                if (!(contributionUpdateDto.DocumentFile.ContentType.Equals("application/pdf") ||
                      contributionUpdateDto.DocumentFile.ContentType.Equals("application/msword") ||
                      contributionUpdateDto.DocumentFile.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document")))
                {
                    throw new InvalidOperationException("The DocumentFile must be a document (PDF or Word).");
                }

                if (!string.IsNullOrEmpty(contribution.FileName))
                {
                    await _fileRepository.DeleteFileAsync(contribution.FileName);
                }

              
                contribution.FileName = await _fileRepository.SaveFileAsync(contributionUpdateDto.DocumentFile);
                contribution.FileType = contributionUpdateDto.DocumentFile.ContentType;
            }

           
            if (contributionUpdateDto.ImageFile != null)
            {
                if (!contributionUpdateDto.ImageFile.ContentType.StartsWith("image/"))
                {
                    throw new InvalidOperationException("The ImageFile must be an image.");
                }

                if (!string.IsNullOrEmpty(contribution.ImageName))
                {
                    await _fileRepository.DeleteFileAsync(contribution.ImageName);
                }

                
                contribution.ImageName = await _fileRepository.SaveFileAsync(contributionUpdateDto.ImageFile);
            }
            contribution.Status = "Updated";

           
            await _contributionRepository.UpdateContributionAsync(contribution);
        }
    }
}
