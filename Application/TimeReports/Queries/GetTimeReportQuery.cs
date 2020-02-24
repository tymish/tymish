using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportQuery : IRequest<TimeReport>
    {
        public TimeReport TimeReport { get; set; }
    }

    public class GetTimeReportHandler : IRequestHandler<GetTimeReportQuery, TimeReport>
    {
        public GetTimeReportHandler() {}
        public Task<TimeReport> Handle(GetTimeReportQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}