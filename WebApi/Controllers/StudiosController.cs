// using System;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Tymish.Application.Invoices.Commands;
// using Tymish.Application.Invoices.Query;

// namespace Tymish.WebApi.Controllers
// {
//     [ApiController]
//     [Route("studios")]
//     public class StudiosController : ControllerBase
//     {
//         private readonly IMediator _mediator;
//         public StudiosController(IMediator mediator)
//         {
//             _mediator = mediator;
//         }

//         [HttpGet()]
//         public async Task<IActionResult> GetInvoice(Guid invoiceId)
//         {
//             var response = await _mediator
//                 .Send(new GetVendorInvoiceQuery{ InvoiceId = invoiceId });
//             return Ok(response);
//         }

//         [HttpGet("{studioId}/invoices", Name="ListInvoices")]
//         public async Task<IActionResult> ListInvoices(Guid studioId)
//         {
//             var response = await _mediator 
//                 .Send(new ListStudioInvoicesQuery{StudioId = studioId});
//             return Ok(response);
//         }

//         [HttpPut("{studioId}/invoices/pay")]
//         public async Task<IActionResult> PayInvoice
//         (
//             Guid studioId,
//             [FromBody] PayInvoiceCommand request
//         )
//         {
//             var response = await _mediator.Send(request);
//             return Ok(response);
//         }
//     }
// }