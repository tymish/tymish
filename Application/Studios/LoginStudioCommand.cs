using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class LoginStudioCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginStudioHandler : IRequestHandler<LoginStudioCommand, string>
    {
        private readonly ITymishDbContext _context;
        private readonly IAuthGateway _auth;
        public LoginStudioHandler(ITymishDbContext context, IAuthGateway auth)
        {
            _context = context;
            _auth = auth;
        }

        public async Task<string> Handle(
            LoginStudioCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context
                .Set<User>()
                .Where(user => 
                    request.Email == user.Email
                    && request.Password == user.Password)
                .SingleOrDefaultAsync(cancellationToken);
            
            return user != default
                ? _auth.GenerateUserToken(user)
                : string.Empty;
        }
    }
}