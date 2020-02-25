using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Application.Exceptions;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;

namespace Tymish.Application.Employees.Commands
{
    public class DeleteEmployeeCommand : IRequest
    {
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
            var employee = dbSet
                .SingleOrDefault(e => e.EmployeeNumber == request.EmployeeNumber);

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