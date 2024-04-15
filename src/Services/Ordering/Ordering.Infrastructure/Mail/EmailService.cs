using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Ordering.Infrastructure.Mail;

public class EmailService : IEmailService
{
    public EmailSettings EmailSettings { get; set; }
    public ILogger<EmailService> Logger { get; set; }
    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        EmailSettings = emailSettings.Value;
        Logger = logger;
    }

    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient(EmailSettings.ApiKey);
        var subject = email.Subject;
        var to = new EmailAddress(email.To);
        var body = email.Body;
        var from = new EmailAddress
        {
            Email = EmailSettings.FromAddress,
            Name = EmailSettings.FromName
        };
        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, body, body);
        var response = await client.SendEmailAsync(sendGridMessage);
        Logger.LogInformation("Email sent.");
        if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
            return true;
        Logger.LogError("Email sending failed.");
        return false;
    }
}
