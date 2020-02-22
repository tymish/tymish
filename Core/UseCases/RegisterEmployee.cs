using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Gateways;
using MediatR;

namespace Core.UseCases
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