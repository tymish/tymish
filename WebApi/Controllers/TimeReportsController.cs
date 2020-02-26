using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.TimeReports.Commands;
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

        [HttpPost(Name="createTimeReport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TimeReport), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateTimeReportCommand request)
        {
            var response = await _mediator.Send(request);
            return Created($"time-reports/{response.Id}", response);
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
