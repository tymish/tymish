using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tymish.Application.Vendors.Commands;

namespace Tymish.WebApi.Controllers
{
    [ApiController]
    [Route("studios")]
    [Produces("application/json")]
    public class StudiosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudiosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login", Name="loginStudio")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginStudioCommand request)
        {
            var response = await _mediator.Send(request);
            if (string.IsNullOrWhiteSpace(response))
                return Unauthorized();
            return Ok(response);
        }
    }
}