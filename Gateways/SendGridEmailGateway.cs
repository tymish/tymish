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

        public async Task Send(string toEmail, string subject, string content)
        {
            var from = new EmailAddress("robot@tymish.app", "Robot");
            var to = new EmailAddress(toEmail);

            var plainTextContent = content;
            var htmlContent = $"<a href=\"{content}\">{content}</a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await _client.SendEmailAsync(msg);
        }
    }
}