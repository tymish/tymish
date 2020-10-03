using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Queries
{
    public class GetVendorQuery : IRequest<Vendor>
    {
        public Guid VendorId { get; set; }
    }

    public class GetVendorHandler : IRequestHandler<GetVendorQuery, Vendor>
    {
        private readonly ITymishDbContext _context;
        public GetVendorHandler(ITymishDbContext context)
        {
            _context = context;
        }
        public async Task<Vendor> Handle(GetVendorQuery request, CancellationToken cancellationToken)
        {
            return await _context
                .Set<Vendor>()
                .SingleOrDefaultAsync(vendor
                    => vendor.Id == request.VendorId,
                    cancellationToken);
        }
    }
}