using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tymish.Domain.Entities;
using Tymish.Application.Employees.Commands;
using Tymish.Application.Employees.Queries;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Name="getEmployeeList")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<Employee>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetEmployeeListQuery());
            return Ok(response);
        }

        [HttpPost(Name="post")]
        public async Task<IActionResult> Post([FromBody] CreateEmployeeCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut(Name="put")]
        public async Task<IActionResult> Put([FromBody] UpdateEmployeeCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete(Name="delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteEmployeeCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
