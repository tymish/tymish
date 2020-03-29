using System.Threading.Tasks;

namespace Tymish.Application.Interfaces
{
    public interface IEmailGateway
    {
        Task Send(string toEmail, string subject, string content);
    }
}