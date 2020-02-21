using Core.Entities;
using Core.Gateways;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Database
{
    public class TymishDbContext : DbContext, ITymishDbContext
    {
        public TymishDbContext() {}

        public TymishDbContext(DbContextOptions options)
            : base(options) {}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeReport> TimeReports { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
    }
}