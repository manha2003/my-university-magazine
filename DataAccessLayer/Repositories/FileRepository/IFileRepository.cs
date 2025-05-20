using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.FileRepository
{
    public interface IFileRepository
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<byte[]> GetFileAsync(string filename);
        Task<bool> DeleteFileAsync(string filename);
        Task<string> ConvertWordToPdfAsync(string originalWordFileName);
    }
}
