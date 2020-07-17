using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class AddVendorCommand : IRequest<Vendor>
    {
        public string Email { get; set; }
        public decimal HourlyPay { get; set; }
    }
    public class AddVendorHandler : IRequestHandler<AddVendorCommand, Vendor>
    {
        private readonly ITymishDbContext _context;
        public AddVendorHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Vendor> Handle(AddVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = new Vendor(request.Email)
            {
                Id = Guid.NewGuid(),
                HourlyPay = request.HourlyPay
            };

            await _context.Set<Vendor>().AddAsync(vendor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return vendor;
        }
    }
}