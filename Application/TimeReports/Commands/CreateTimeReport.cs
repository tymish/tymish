using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Tymish.Application.TimeReports.Commands
{
    public class CreateTimeReportCommand : IRequest<TimeReport>
    {
        public DateTime EmployeeNumber { get; set; }
        public IList<TimeEntry> TimeEntries { get; set; }
    }

    public class CreateTimeReportHandler : IRequestHandler<CreateTimeReportCommand, TimeReport>
    {
        public CreateTimeReportHandler() {}
        public Task<TimeReport> Handle(CreateTimeReportCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}