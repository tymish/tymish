/* 
 * This is the only place that Entity FrameworkCore should be used
 * It would be nice to one day remove the depenceny and only use MediatR.
 */
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tymish.Application.Interfaces
{
    public interface ITymishDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}