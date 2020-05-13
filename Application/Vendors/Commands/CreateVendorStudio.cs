using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Commands
{
    public class CreateVendorStudioCommand : IRequest<VendorStudio>
    {
        public string VendorId { get; set; }
        public Guid StudioId { get; set; }
        public decimal HourlyPay { get; set; }
    }
    public class CreateVendorStudioHandler : IRequestHandler<CreateVendorStudioCommand, VendorStudio>
    {
        private ITymishDbContext _context;
        public CreateVendorStudioHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<VendorStudio> Handle(CreateVendorStudioCommand request, CancellationToken cancellationToken)
        {
            var vendor = new VendorStudio
            {
                VendorId = request.VendorId,
                StudioId = request.StudioId,
                HourlyPay = request.HourlyPay
            };

            await _context.Set<VendorStudio>().AddAsync(vendor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return vendor;
        }
    }
}