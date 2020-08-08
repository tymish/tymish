using Tymish.Domain.Entities;

namespace Tymish.Application.Interfaces
{
    public interface IAuthGateway
    {
        string GenerateVendorToken(Vendor vendor);
    }
}