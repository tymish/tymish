using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using MediatR;

namespace Core.UseCases
{
    public class RegisterEmployee : IRequest<Employee>
    {
        public Employee Employee { get; set; }
    }

    public class RegisterEmployeeHandler : IRequestHandler<RegisterEmployee, Employee>
    {
        public RegisterEmployeeHandler() {}
        public Task<Employee> Handle(RegisterEmployee request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Employee>(request.Employee);
        }
    }
}