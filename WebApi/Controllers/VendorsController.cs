using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Invoices.Commands;
using Tymish.Application.Invoices.Query;
using Tymish.Application.Vendors.Commands;
using Tymish.Domain.Entities;

namespace Tymish.WebApi.Controllers
{
    [ApiController]
    [Route("vendors")]
    public class VendorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name="createVendorStudio")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VendorStudio), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateVendorStudioCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{vendorId}/invoices", Name="getInvoices")]
        [ProducesResponseType(typeof(IList<VendorInvoice>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListInvoices(string vendorId)
        {
            var response = await _mediator
                .Send(new GetVendorInvoices{ VendorId = vendorId });
            return Ok();
        }

        [HttpGet("{vendorId}/invoices/{invoiceId}", Name="getInvoice")]
        public async Task<IActionResult> GetInvoice(string vendorId, Guid invoiceId)
        {
            return Ok();
        }

        [HttpPost("{vendorId}/invoices/submit", Name="submitInvoice")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status201Created)]
        public async Task<IActionResult> SubmitInvoice(string vendorId, [FromBody] SubmitInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Created($"/{vendorId}/invoices/{response.Id}", response);
        }
    }
}