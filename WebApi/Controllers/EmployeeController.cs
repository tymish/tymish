using System;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tymish.Application.Employees;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<Employee>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator
                .Send(new GetEmployeeListQuery());
                
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            var response = await _mediator
                .Send(new CreateEmployeeCommand()
                {
                    Employee = employee
                });
            return Ok(response);
        }
    }
}
