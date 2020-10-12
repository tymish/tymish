using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly ITymishDbContext _context;
        private readonly IAuthGateway _auth;
        public LoginHandler(ITymishDbContext context, IAuthGateway auth)
        {
            _context = context;
            _auth = auth;
        }

        public async Task<string> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var vendor = await _context
                .Set<Vendor>()
                .Where(vendor => 
                    request.Email == vendor.Email
                    && request.Password == vendor.Password)
                .SingleOrDefaultAsync(cancellationToken);
            
            return vendor != default
                ? _auth.GenerateVendorToken(vendor)
                : string.Empty;
        }
    }
}