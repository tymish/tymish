using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Dtos;
using Tymish.Application.Invoices.Commands;
using Tymish.Application.Invoices.Query;
using Tymish.Domain.Entities;

namespace Tymish.WebApi.Controllers
{
    [ApiController]
    [Route("invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}", Name="getInvoiceById")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetInvoiceQuery(id));
            return Ok(response);
        }

        [HttpGet(Name="listInvoices")]
        public async Task<IActionResult> ListInvoices([FromQuery] string status)
        {
            var response = await _mediator.Send(new ListInvoicesQuery());
            return Ok(response);
        }


        [HttpPost("{id:guid}/pay", Name="payInvoice")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> PayInvoice(
            [FromRoute] Guid id,
            [FromBody] PayInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
