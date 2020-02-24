using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tymish.Persistence
{
    public class TymishDbContext : DbContext, ITymishDbContext
    {
        public TymishDbContext() {}

        public TymishDbContext(DbContextOptions options)
            : base(options) {}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeReport> TimeReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(TymishDbContext).Assembly);
        }
    }
}