using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tymish.Application.Dtos;
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
        private readonly IConfiguration _config;

        public VendorsController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }

        #region GETs
        [HttpGet("{id}", Name="getVendor")]
        [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVendor(Guid id)
        {
            var request = new GetVendorQuery{VendorId = id};
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet(Name="listVendors")]
        [ProducesResponseType(typeof(IList<Invoice>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListVendors()
        {
            var response = await _mediator.Send(new ListVendorsQuery());
            return Ok(response);
        }

        [HttpGet("{id}/invoices", Name="listVendorInvoices")]
        [ProducesResponseType(typeof(IList<InvoiceDto>), StatusCodes.Status200OK)]
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
            request.VendorSiteDomain = _config.GetValue<string>("VendorDomain");
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("register", Name="registerVendor")]
        public async Task<IActionResult> RegisterVendor(RegisterVendorCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("login", Name="loginVendor")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginVendorCommand request)
        {
            var response = await _mediator.Send(request);
            if (string.IsNullOrWhiteSpace(response))
                return Unauthorized();
            return Ok(response);
        }
    }
}