using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportsByMonthQuery : IRequest<IList<TimeReport>>
    {
        public int IssuedMonth { get; set; }
        public int IssuedYear { get; set; }
    }

    public class GetTimeReportsByMonthHandler : IRequestHandler<GetTimeReportsByMonthQuery, IList<TimeReport>>
    {
        private readonly ITymishDbContext _context;

        public GetTimeReportsByMonthHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<TimeReport>> Handle(GetTimeReportsByMonthQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<TimeReport>()
                .Where(e
                    => e.Issued.Month == request.IssuedMonth
                    && e.Issued.Year == request.IssuedYear)
                .ToListAsync();

            return entity;
        }
    }
}