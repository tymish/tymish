using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Dtos;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Queries
{
    public class GetVendorQuery : IRequest<VendorDto>
    {
        public Guid VendorId { get; set; }
    }

    public class GetVendorHandler : IRequestHandler<GetVendorQuery, VendorDto>
    {
        private readonly ITymishDbContext _context;
        private readonly IMapper _mapper;
        public GetVendorHandler(
            ITymishDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<VendorDto> Handle(GetVendorQuery request, CancellationToken cancellationToken)
        {
            var vendor = await _context
                .Set<Vendor>()
                .SingleOrDefaultAsync(vendor
                    => vendor.Id == request.VendorId,
                    cancellationToken);

            return _mapper.Map<VendorDto>(vendor);
        }
    }
}