using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Vendors.Queries
{
    public class ListVendorsQuery : IRequest<IList<Vendor>> {}

    public class ListVendorsHandler : IRequestHandler<ListVendorsQuery, IList<Vendor>>
    {
        private readonly ITymishDbContext _context;
        public ListVendorsHandler(ITymishDbContext context)
        {
            _context = context;
        }
        public async Task<IList<Vendor>> Handle(ListVendorsQuery request, CancellationToken cancellationToken)
        {
            return await _context
                .Set<Vendor>()
                .ToListAsync(cancellationToken);
        }
    }
}