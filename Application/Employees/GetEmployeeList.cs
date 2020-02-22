using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;

namespace Tymish.Application.Employees
{
    public class GetEmployeeListQuery : IRequest<IList<Employee>>
    {
        
    }
    public class GetEmployeeListHandler : IRequestHandler<GetEmployeeListQuery, IList<Employee>>
    {
        private readonly ITymishDbContext _context;
        public GetEmployeeListHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Employee>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Set<Employee>().ToListAsync(cancellationToken);
        }
    }
}