using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class RegisterVendorCommand : IRequest<Vendor>
    {
        public Guid VendorId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MobilePhone { get; set; }
    }
    public class RegisterVendorHandler : IRequestHandler<RegisterVendorCommand, Vendor>
    {
        private readonly ITymishDbContext _context;
        public RegisterVendorHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Vendor> Handle(RegisterVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _context
                .Set<Vendor>()
                .SingleOrDefaultAsync(vendor
                    => vendor.Id == request.VendorId,
                    cancellationToken);

            vendor.GivenName = request.GivenName;
            vendor.FamilyName = request.FamilyName;
            vendor.MobilePhone = request.MobilePhone;

            _context.Set<Vendor>().Update(vendor);            
            await _context.SaveChangesAsync(cancellationToken);

            return vendor;
        }
    }
}