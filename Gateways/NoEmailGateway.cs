using System.Threading.Tasks;
using Tymish.Application.Interfaces;

namespace Tymish.Gateways
{
    public class NoEmailGateway : IEmailGateway
    {
        public async Task Send(string toEmail, string body)
        {
            // nothing
        }
    }
}