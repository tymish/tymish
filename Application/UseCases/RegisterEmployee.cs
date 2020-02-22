using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;
using MediatR;

namespace Tymish.Application.UseCases
{
    public class RegisterEmployeeCommand : IRequest
    {
        public Employee Employee { get; set; }
    }

    public class RegisterEmployeeHandler : IRequestHandler<RegisterEmployeeCommand, Unit>
    {
        private ITymishDbContext _context;
        public RegisterEmployeeHandler(ITymishDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Employee;

            _context.Set<Employee>().Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}