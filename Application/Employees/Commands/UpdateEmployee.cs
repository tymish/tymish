using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;

namespace Tymish.Application.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest
    {
        public int EmployeeNumber { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public decimal HourlyPay { get; set; }
    }

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        private readonly ITymishDbContext _context;

        public UpdateEmployeeHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dbSet = _context.Set<Employee>();

            var employee = dbSet
                .SingleOrDefault(e => e.EmployeeNumber == request.EmployeeNumber);

            // TODO: This must be done better to handle scenarios
            employee.GivenName = request.GivenName;
            employee.FamilyName = request.FamilyName;
            employee.Email = request.Email;
            employee.HourlyPay = request.HourlyPay;

            dbSet.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}