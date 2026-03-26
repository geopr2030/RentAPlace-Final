using Microsoft.Extensions.Logging;
using RentAPlace.API.Interfaces;

namespace RentAPlace.API.Services
{
    /// <summary>
    /// For the capstone project, this is a mock Email Service that logs to the console.
    /// In production, you would use MailKit, SendGrid, or System.Net.Mail here.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Simulate email sending delay
            _logger.LogInformation("===========================================");
            _logger.LogInformation($"[EMAIL SENT TO]: {toEmail}");
            _logger.LogInformation($"[SUBJECT]: {subject}");
            _logger.LogInformation($"[BODY]: {body}");
            _logger.LogInformation("===========================================");

            return Task.CompletedTask;
        }
    }
}
