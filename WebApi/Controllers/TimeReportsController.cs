using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Dtos;
using Tymish.Application.Employees.Queries;
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

        [HttpGet("{id:guid}/employee", Name="getEmployeeByTimeReportId")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var response = await _mediator
                .Send(new GetEmployeeByTimeReportIdQuery(id));
            return Ok(response);
        }

        [HttpGet(Name="getEmployeeTimeReportAggregates")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<EmployeeTimeReportAggregateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] DateTime month)
        {
            var response = await _mediator
                .Send(new GetEmployeeTimeReportAggregatesQuery 
                {
                    Sent = month
                });
            return Ok(response);
        }

        [HttpGet("summary", Name="getMonthAggregate")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MonthlyAggregateDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime month)
        {
            var response = await _mediator
                .Send(new GetMonthlyAggregateQuery
                {
                    Sent = month
                });
            return Ok(response);
        }

        [HttpPut("submit", Name="submitTimeReport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TimeReport), StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitTimeReport([FromBody] SubmitTimeReportCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("pay", Name="payTimeReport")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TimeReport), StatusCodes.Status200OK)]
        public async Task<IActionResult> PayTimeReport([FromBody] PayTimeReportCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("sent", Name="sendTimeReports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendTimeReports([FromBody] SendTimeReportsCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
