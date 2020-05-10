using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class CreateVendorCommand : IRequest<Vendor>
    {
        public decimal HourlyRate { get; set; }
        public Guid StudioId { get; set; }
    }
    public class CreateVendorHandler : IRequestHandler<CreateVendorCommand, Vendor>
    {
        private ITymishDbContext _context;
        public CreateVendorHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Vendor> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            var studio = await _context.Set<Studio>()
                .FindAsync(request.StudioId, cancellationToken);

            var vendor = new Vendor
            {
                HourlyRate = request.HourlyRate,
            };

            vendor.Studios.Add(studio);

            await _context.SaveChangesAsync(cancellationToken);

            return vendor;
        }
    }
}