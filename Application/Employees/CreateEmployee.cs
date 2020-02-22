using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;
using MediatR;

namespace Tymish.Application.Employees
{
    public class CreateEmployeeCommand : IRequest
    {
        public Employee Employee { get; set; }
    }

    public class RegisterEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Unit>
    {
        private ITymishDbContext _context;
        public RegisterEmployeeHandler(ITymishDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Employee;

            _context.Set<Employee>().Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}