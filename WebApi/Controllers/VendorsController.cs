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

        #region GETs
        [HttpGet(Name="listVendors")]
        public async Task<IActionResult> ListVendors()
        {
            var response = await _mediator.Send(null);
            return Ok(Response);
        }

        [HttpGet("{vendorId}/invoices", Name="listVendorInvoices")]
        [ProducesResponseType(typeof(IList<VendorInvoice>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListVendorInvoices(string vendorId)
        {
            var response = await _mediator
                .Send(new ListVendorInvoicesQuery{ VendorId = vendorId });
            return Ok(response);
        }

        [HttpGet("{vendorId}/invoices/{invoiceId}", Name="getInvoice")]
        public async Task<IActionResult> GetInvoice(string vendorId, Guid invoiceId)
        {
            var response = await _mediator
                .Send(new GetVendorInvoiceQuery{ InvoiceId = invoiceId });
            return Ok(response);
        }
        #endregion
        [HttpPost(Name="addVendor")]
        public async Task<IActionResult> AddVendor()
        {
            return Accepted("", null);
        }

        [HttpPost("{vendorId}/register", Name="registerVendor")]
        public async Task<IActionResult> RegisterVendor()
        {
            return Created("", null);
        }

        [HttpPost("{vendorId}/invoices", Name="submitVendorInvoice")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status201Created)]
        public async Task<IActionResult> SubmitInvoice(string vendorId, [FromBody] SubmitInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Created($"/{vendorId}/invoices/{response.Id}", response);
        }

        [HttpPost(Name="createVendorStudio")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VendorStudio), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateVendorStudioCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}