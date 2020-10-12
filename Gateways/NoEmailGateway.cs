using System;
using System.Threading.Tasks;
using Tymish.Application.Interfaces;

namespace Tymish.Gateways
{
    public class NoEmailGateway : IEmailGateway
    {
        public async Task Send(string toEmail, string subject, string content)
        {
            await Task.Run(()
                => Console.WriteLine($"No email sent. {toEmail}, subject: {subject}, content: {content}")
            );
        }
    }
}