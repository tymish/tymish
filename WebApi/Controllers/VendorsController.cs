using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Invoices.Query;
using Tymish.Application.Vendors.Commands;
using Tymish.Application.Vendors.Queries;
using Tymish.Domain.Entities;

namespace Tymish.WebApi.Controllers
{
    [ApiController]
    [Route("vendors")]
    [Produces("application/json")]
    public class VendorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region GETs
        [HttpGet(Name="listVendors")]
        [ProducesResponseType(typeof(IList<Invoice>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListVendors()
        {
            var response = await _mediator.Send(new ListVendorsQuery());
            return Ok(Response);
        }

        [HttpGet("{id}/invoices", Name="listVendorInvoices")]
        [ProducesResponseType(typeof(IList<Invoice>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListInvoices(Guid id)
        {
            var request = new ListInvoicesForVendorQuery(id);
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        #endregion

        [HttpPost(Name="addVendor")]
        [ProducesResponseType(typeof(Vendor), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddVendor([FromBody] AddVendorCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("register", Name="registerVendor")]
        public async Task<IActionResult> RegisterVendor(RegisterVendorCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("login", Name="login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}