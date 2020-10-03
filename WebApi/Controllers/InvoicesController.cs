using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Invoices.Commands;
using Tymish.Application.Invoices.Query;
using Tymish.Domain.Entities;

namespace Tymish.WebApi.Controllers
{
    [ApiController]
    [Route("invoices")]
    [Produces("application/json")]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name="getInvoiceById")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var request = new GetInvoiceQuery(id);
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet(Name="listInvoices")]
        public async Task<IActionResult> ListInvoices([FromQuery] string status)
        {
            var request = new ListInvoicesQuery();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("submit", Name="submitInvoice")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitInvoice([FromBody] SubmitInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("pay", Name="payInvoice")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> PayInvoice([FromBody] PayInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
