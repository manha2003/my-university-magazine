using System;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Validator;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories.AcademicTermRepository;
using AutoMapper;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.AcademicTermService
{
    public class AcademicTermService : IAcademicTermService
    {
        private readonly IAcademicTermRepository _academicTermRepository;
        private readonly IAcademicTermValidator _academicTermValidator;
        private readonly IMapper _mapper;

        public AcademicTermService(IAcademicTermRepository academicTermRepository,IAcademicTermValidator academicTermValidator, IMapper mapper)
        {
            _academicTermRepository = academicTermRepository;
            _academicTermValidator = academicTermValidator;
            _mapper = mapper;
        }

        public async Task AddAcademicTermAsync(AcademicTermDto academicTermDto)
        {
            var isValidAcademicYear = _academicTermValidator.ValidateAcademicYear(academicTermDto.AcademicYear, out string errorMessage);
            if (!isValidAcademicYear)
            {
                throw new InvalidOperationException(errorMessage);
            }
            DateTime entryDate, closureDate, finalClosureDate;

            bool isEntryDateValid = DateTime.TryParse(academicTermDto.EntryDate, out entryDate);
            bool isClosureDateValid = DateTime.TryParse(academicTermDto.ClosureDate, out closureDate);
            bool isFinalClosureDateValid = DateTime.TryParse(academicTermDto.FinalClosure, out finalClosureDate);

            if (!isEntryDateValid || !isClosureDateValid || !isFinalClosureDateValid)
            {
                throw new FormatException("One or more dates are in an invalid format.");
            }


            if (entryDate < DateTime.Now)
            {
                throw new InvalidOperationException("Entry Date cannot be in the past.");
            }

            if (closureDate <= entryDate)
            {
                throw new InvalidOperationException("Closure Date must be after Entry Date.");
            }

            if (finalClosureDate <= closureDate)
            {
                throw new InvalidOperationException("Final Closure Date must be after Closure Date.");
            }

            

            var academicTerm = _mapper.Map<AcademicTerm>(academicTermDto);
            academicTerm.EntryDate = entryDate;
            academicTerm.ClosureDate = closureDate;
            academicTerm.FinalClosure = finalClosureDate;

            await _academicTermRepository.AddAcademicTermAsync(academicTerm);
        }

        

        public async Task<AcademicTermDetailsDto> GetAcademicTermByIdAsync(int academicTermId)
        {
            var academicTerm = await _academicTermRepository.GetAcademicTermByIdAsync(academicTermId);

            return _mapper.Map<AcademicTermDetailsDto>(academicTerm);
        }

        public async Task<List<AcademicTermDetailsDto>> GetAllAcademicTermsAsync()
        {
            var academicTerms = await _academicTermRepository.GetAllAcademicTermsAsync();
            return _mapper.Map<List<AcademicTermDetailsDto>>(academicTerms);
        }
        public async Task<AcademicTermDetailsDto> GetMostRecentlyEndedTermAsync()
        {
            var academicTerms = await _academicTermRepository.GetMostRecentlyEndedTermAsync();
            return _mapper.Map<AcademicTermDetailsDto>(academicTerms);
        }
        
    }
}
