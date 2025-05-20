using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Aspose.Words;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.FileRepository
{
    public class FileRepository : IFileRepository
    {
       
        private readonly string _filePath;
        public FileRepository(IConfiguration configuration)
        {

            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

            
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            var filename = GenerateFileName(file.FileName);
            var savePath = Path.Combine(_filePath, filename);

            await using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filename;
        }

        public async Task<byte[]> GetFileAsync(string filename)
        {
            var filePath = Path.Combine(_filePath, filename);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filename);
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<bool> DeleteFileAsync(string filename)
        {
            var filePath = Path.Combine(_filePath, filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
        public async Task<string> ConvertWordToPdfAsync(string originalWordFileName)
        {
            var originalFilePath = Path.Combine(_filePath, originalWordFileName);
            if (!File.Exists(originalFilePath))
            {
                throw new FileNotFoundException("Original Word file not found.", originalWordFileName);
            }

            var pdfFileName = Path.ChangeExtension(originalWordFileName, ".pdf");
            var pdfFilePath = Path.Combine(_filePath, pdfFileName);

            // Load the document
            var document = new Aspose.Words.Document(originalFilePath);

            // Save the document as PDF
            document.Save(pdfFilePath, SaveFormat.Pdf);

            // Optionally, return the path or name of the PDF file
            return pdfFileName;
        }

        private string GenerateFileName(string originalFileName)
        {
            // Use a GUID to ensure the filename is unique
            // You might want to retain the original file's extension
            var extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}
