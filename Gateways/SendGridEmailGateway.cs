using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Tymish.Application.Interfaces;

namespace Tymish.Gateways
{
    public class SendGridEmailGateway : IEmailGateway
    {
        private ISendGridClient _client;
        public SendGridEmailGateway(string apiKey)
        {
            _client = new SendGridClient(apiKey);
        }

        public async Task Send(string toEmail, string body)
        {
            var from = new EmailAddress("test@example.com", "Example");
            var subject = "Sending with sendgrid";
            var to = new EmailAddress(toEmail);
            var plainTextContent = body;
            
            var htmlContent = "<strong>html is strong</strong>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await _client.SendEmailAsync(msg);
        }
    }
}