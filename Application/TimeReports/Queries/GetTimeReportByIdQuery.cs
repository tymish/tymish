using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportByIdQuery : IRequest<TimeReport>
    {
        public GetTimeReportByIdQuery(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; set; }
    }

    public class GetTimeReportByIdHandler : IRequestHandler<GetTimeReportByIdQuery, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public GetTimeReportByIdHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<TimeReport> Handle(GetTimeReportByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<TimeReport>()
                .SingleOrDefaultAsync(e => e.Id == request.Id);

            if (entity == default(TimeReport))
            {
                throw new NotFoundException(nameof(TimeReport), request.Id);
            }

            return entity;
        }
    }
}