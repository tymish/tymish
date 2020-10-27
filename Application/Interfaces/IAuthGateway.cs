using System.Threading.Tasks;
using Tymish.Domain.Entities;

namespace Tymish.Application.Interfaces
{
    public class AuthOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpiryDays { get; set; }
    }
    public interface IAuthGateway
    {
        string GenerateVendorToken(Vendor vendor);
        string GenerateUserToken(User user);
    }
}