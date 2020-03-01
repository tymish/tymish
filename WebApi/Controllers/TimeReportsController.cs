using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.TimeReports.Commands;
using Tymish.Application.TimeReports.Query;
using Tymish.Domain.Entities;

namespace Tymish.WebApi.Controllers
{
    [ApiController]
    [Route("time-reports")]
    public class TimeReportsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TimeReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}", Name="getTimeReportById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TimeReport), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetTimeReportByIdQuery(id));
            return Ok(response);
        }

        [HttpGet(Name="getTimeReportByIssuedMonth")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<TimeReport>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year)
        {
            var response = await _mediator
                .Send(new GetTimeReportsByMonthQuery 
                {
                    IssuedMonth = month,
                    IssuedYear = year
                });
            return Ok(response);
        }

        [HttpPut("{id:guid}/submitted", Name="submitTimeReport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TimeReport), StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitTimeReport([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new SubmitTimeReportCommand{Id = id});
            return Ok(response);
        }

        [HttpPost("bulk", Name="createTimeReports")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Array), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CreateTimeReports([FromBody] CreateTimeReportsCommand request)
        {
            var response = await _mediator.Send(request);
            return Accepted(response);
        }

        [HttpPut("bulk/issued", Name="issueTimeReports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IssueTimeReports([FromBody] IssueTimeReportsCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
