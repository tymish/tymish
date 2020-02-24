using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;
using MediatR;

namespace Tymish.Application.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public decimal HourlyPay { get; set; }
    }

    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Unit>
    {
        private ITymishDbContext _context;

        public CreateEmployeeHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Employee 
            {
                Id = new Guid(),
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                Email = request.Email,
                HourlyPay = request.HourlyPay
            };

            _context.Set<Employee>().Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}