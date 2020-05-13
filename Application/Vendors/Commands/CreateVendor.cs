using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class CreateVendorCommand : IRequest<VendorStudio>
    {
        public decimal HourlyRate { get; set; }
        public Guid StudioId { get; set; }
    }
    public class CreateVendorHandler : IRequestHandler<CreateVendorCommand, VendorStudio>
    {
        private ITymishDbContext _context;
        public CreateVendorHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<VendorStudio> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            var studio = await _context.Set<Studio>()
                .FindAsync(request.StudioId, cancellationToken);

            var vendor = new VendorStudio
            {
                HourlyPay = request.HourlyRate,
            };

            vendor.Studios.Add(studio);

            await _context.SaveChangesAsync(cancellationToken);

            return vendor;
        }
    }
}