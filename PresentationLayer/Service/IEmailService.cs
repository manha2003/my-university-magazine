using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Service
{ 
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailrequest, ContributionDetailsDto contributionDto);
    }
}
