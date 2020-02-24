using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;

namespace Tymish.Application.TimeReports.Commands
{
    public class CreateTimeReportCommand : IRequest<TimeReport>
    {
        public TimeReport TimeReport { get; set; }
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