using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using Tymish.Domain.Entities;
using Tymish.Application.Interfaces;

namespace Tymish.Application.Employees.Commands
{
    public class DeleteEmployeeCommand : IRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int EmployeeNumber { get; set; }   
    }

    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly ITymishDbContext _context;

        public DeleteEmployeeHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dbSet = _context.Set<Employee>();
            var employee = await dbSet.SingleOrDefaultAsync(
                e => e.EmployeeNumber == request.EmployeeNumber,
                cancellationToken
            );

            if (employee == default(Employee))
            {
                throw new NotFoundException(nameof(Employee), request.EmployeeNumber);
            }
            
            dbSet.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}