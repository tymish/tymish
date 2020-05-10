using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost(Name="createVendor")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Vendor), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateVendorCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}