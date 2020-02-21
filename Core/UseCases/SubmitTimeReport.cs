using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using MediatR;

namespace Core.UseCases
{
    public interface SubmitTimeReport : IRequest<TimeReport>
    {
        public TimeReport TimeReport { get; set; }
    }

    public class SubmitTimeReportHandler : IRequestHandler<SubmitTimeReport, TimeReport>
    {
        public SubmitTimeReportHandler() {}
        public Task<TimeReport> Handle(SubmitTimeReport request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}