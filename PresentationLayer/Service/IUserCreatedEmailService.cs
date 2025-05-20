using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PresentationLayer.Service
{
    public interface IUserCreatedEmailService
    {
        Task SendEmailAsync(MailRequest mailrequest);
    }
}
