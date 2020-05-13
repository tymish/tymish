using System;
using System.Collections.Generic;
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetInvoiceByIdQuery(id));
            return Ok(response);
        }

        [HttpPut("pay", Name="payInvoice")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> PayInvoice([FromBody] PayInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("{id:guid}/sent", Name="sendInvoice")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendInvoice([FromRoute] Guid invoiceId)
        {
            var response = await _mediator.Send(new SendInvoiceCommand{InvoiceId = invoiceId});
            return Ok(response);
        }
    }
}
