using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Commands
{
    public class PayTimeReportCommand : IRequest<TimeReport>
    {

    }

    public class PayTimeReportHandler : IRequestHandler<PayTimeReportCommand, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public PayTimeReportHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<TimeReport> Handle(PayTimeReportCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}