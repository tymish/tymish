using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class RegisterVendorCommand : IRequest<string>
    {
        public Guid VendorId { get; set; }
        public string Password { get; set; }
        public string MobilePhone { get; set; }
    }
    public class RegisterVendorHandler : IRequestHandler<RegisterVendorCommand, string>
    {
        private readonly ITymishDbContext _context;
        private readonly IAuthGateway _auth;
        public RegisterVendorHandler(
            ITymishDbContext context,
            IAuthGateway auth)
        {
            _context = context;
            _auth = auth;
        }

        public async Task<string> Handle(RegisterVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _context
                .Set<Vendor>()
                .SingleOrDefaultAsync(vendor
                    => vendor.Id == request.VendorId,
                    cancellationToken);

            vendor.MobilePhone = request.MobilePhone;
            vendor.Password = request.Password;
            vendor.Registered = DateTime.Now;

            _context.Set<Vendor>().Update(vendor);            
            await _context.SaveChangesAsync(cancellationToken);

            var jwt = _auth.GenerateVendorToken(vendor);

            return jwt;
        }
    }
}