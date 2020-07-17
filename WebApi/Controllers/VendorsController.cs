using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Invoices.Commands;
using Tymish.Application.Invoices.Query;
using Tymish.Application.Vendors.Commands;
using Tymish.Application.Vendors.Queries;
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
            var response = await _mediator.Send(new ListVendorsQuery());
            return Ok(Response);
        }

        [HttpGet("{vendorId}/invoices", Name="listInvoices")]
        [ProducesResponseType(typeof(IList<Invoice>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListInvoices(Guid vendorId)
        {
            var response = await _mediator
                .Send(new ListInvoicesForVendorQuery(vendorId));
            return Ok(response);
        }
        #endregion


        [HttpPost("{vendorId}/register", Name="registerVendor")]
        public async Task<IActionResult> RegisterVendor(Guid vendorId)
        {
            return Created("", null);
        }

        [HttpPost("{vendorId}/invoices", Name="submitVendorInvoice")]
        [ProducesResponseType(typeof(Invoice), StatusCodes.Status201Created)]
        public async Task<IActionResult> SubmitInvoice(Guid vendorId, [FromBody] SubmitInvoiceCommand request)
        {
            var response = await _mediator.Send(request);
            return Created($"/{vendorId}/invoices/{response.Id}", response);
        }

        [HttpPost(Name="addVendor")]
        [ProducesResponseType(typeof(Vendor), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddVendor([FromBody] AddVendorCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}