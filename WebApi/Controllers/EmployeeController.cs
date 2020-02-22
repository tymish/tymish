using System;
using Core.Entities;
using Core.UseCases;
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
        public IActionResult Post([FromBody] Employee employee)
        {
            var response = _mediator
                .Send(new RegisterEmployee()
                {
                    Employee = employee
                });
            return Ok(response);
        }
    }
}
