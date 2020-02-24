using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;

namespace Tymish.Application.TimeReports.Commands
{
    public interface SubmitTimeReportCommand : IRequest<TimeReport>
    {
        public TimeReport TimeReport { get; set; }
    }

    public class SubmitTimeReportHandler : IRequestHandler<SubmitTimeReportCommand, TimeReport>
    {
        public SubmitTimeReportHandler() {}
        public Task<TimeReport> Handle(SubmitTimeReportCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}