using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using PresentationLayer.Service;
using DataAccessLayer.Repositories.FileRepository;

namespace PresentationLayer.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly IFileRepository _fileRepository;
        public EmailService(IOptions<EmailSettings> options, IFileRepository fileRepository)
        {
            emailSettings = options.Value;
            _fileRepository = fileRepository;
        }
        /*public async Task SendEmailAsync(MailRequest mailrequest, ContributionDetailsDto contributionDto)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();



           
            if (contributionDto.DocumentFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    await contributionDto.DocumentFile.CopyToAsync(ms);
                    byte[] fileBytes = ms.ToArray();
                    builder.Attachments.Add(contributionDto.DocumentFile.FileName, fileBytes, ContentType.Parse(contributionDto.DocumentFile.ContentType));
                }
            }

            // Optionally attach image file
            if (contributionDto.ImageFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    await contributionDto.ImageFile.CopyToAsync(ms);
                    byte[] fileBytes = ms.ToArray();
                    builder.Attachments.Add(contributionDto.ImageFile.FileName, fileBytes, ContentType.Parse(contributionDto.ImageFile.ContentType));
                }
            }

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
*/
        public async Task SendEmailAsync(MailRequest mailRequest, ContributionDetailsDto contributionDto)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();

            try
            {
                // Attach document file
                var documentBytes = await _fileRepository.GetFileAsync(contributionDto.FileName);
                if (documentBytes != null)
                {
                    builder.Attachments.Add(contributionDto.FileName, documentBytes, new ContentType("application", "octet-stream"));
                }

                // Attach image file (assuming you have an image file's name in ContributionDetailsDto)
                if (!string.IsNullOrEmpty(contributionDto.ImageName))
                {
                    var imageBytes = await _fileRepository.GetFileAsync(contributionDto.ImageName);
                    builder.Attachments.Add(contributionDto.ImageName, imageBytes, new ContentType("image", "jpeg")); // Adjust the ContentType based on your actual image file type
                }

                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSettings.Email, emailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
        }
    }
}
