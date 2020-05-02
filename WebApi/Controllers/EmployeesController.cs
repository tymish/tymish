using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tymish.Domain.Entities;
using Tymish.Application.Employees.Commands;
using Tymish.Application.Employees.Queries;
using System;
using Tymish.Application.Invoices.Commands;
using Tymish.Application.Invoices.Query;

namespace Tymish.WebApi.Controllers
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
        public async Task<IActionResult> GetEmployeeList()
        {
            var response = await _mediator.Send(new GetEmployeeListQuery());
            return Ok(response);
        }

        [HttpGet("{id:Guid}", Name="getEmployeeById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return Ok(response);
        }

        [HttpGet("{number:int}", Name="getEmployeeByNumber")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByNumber([FromRoute] int number)
        {
            var response = await _mediator.Send(new GetEmployeeByNumberQuery(number));
            return Ok(response);
        }

        [HttpGet("{number:int}/invoices", Name="getInvoicesForEmployee")]
        public async Task<IActionResult> GetInvoicesForEmployee([FromRoute] int number)
        {
            var request = new GetInvoicesByEmployeeNumberQuery
            {
                EmployeeNumber = number
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost(Name="createEmployee")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateEmployeeCommand request)
        {
            var response = await _mediator.Send(request);
            return Created($"/employees/{response.EmployeeNumber}", response);
        }

        [HttpPut(Name="updateEmployee")]
        public async Task<IActionResult> Put([FromBody] UpdateEmployeeCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete(Name="deleteEmployee")]
        public async Task<IActionResult> Delete([FromBody] DeleteEmployeeCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
