using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.DTOs;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.AcademicTermService
{
    public interface IAcademicTermService
    {
        Task AddAcademicTermAsync(AcademicTermDto academicTermDto);
       // Task UpdateAcademicTermAsync(int academicTermId, AcademicTermDto academicTermDto);
        Task<AcademicTermDetailsDto> GetAcademicTermByIdAsync(int academicTermId);
        Task<List<AcademicTermDetailsDto>> GetAllAcademicTermsAsync();
        Task<AcademicTermDetailsDto> GetMostRecentlyEndedTermAsync();
    }
}
