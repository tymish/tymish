using System;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using Tymish.Application.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IActionResult Get()
        {
            return Ok("alive");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            var response = await _mediator
                .Send(new RegisterEmployeeCommand()
                {
                    Employee = employee
                });
            return Ok(response);
        }
    }
}
