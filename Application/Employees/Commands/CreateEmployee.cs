using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using Tymish.Application.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tymish.Application.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<Employee>
    {
        [Required]
        public string GivenName { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Range(0, 100)]
        public decimal HourlyPay { get; set; }
    }

    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Employee>
    {
        private ITymishDbContext _context;

        public CreateEmployeeHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Employee 
            {
                Id = Guid.NewGuid(),
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                Email = request.Email,
                HourlyPay = request.HourlyPay
            };

            _context.Set<Employee>().Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}