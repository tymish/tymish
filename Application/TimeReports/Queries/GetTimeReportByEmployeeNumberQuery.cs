using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportByEmployeeNumberQuery : IRequest<TimeReport>
    {
        public int EmployeeNumber { get; set; }
    }

    public class GetTimeReportByEmployeeNumberHandler : IRequestHandler<GetTimeReportByEmployeeNumberQuery, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public GetTimeReportByEmployeeNumberHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<TimeReport> Handle(GetTimeReportByEmployeeNumberQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<TimeReport>()
                .SingleOrDefaultAsync(e => e.Employee.EmployeeNumber == request.EmployeeNumber);

            if (entity == default(TimeReport))
            {
                throw new NotFoundException(nameof(TimeReport), request.EmployeeNumber);
            }

            return entity;
        }
    }
}