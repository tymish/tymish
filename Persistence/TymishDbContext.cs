using Tymish.Domain.Entities;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tymish.Persistence
{
    public class TymishDbContext : DbContext, ITymishDbContext
    {
        public TymishDbContext() { }

        public TymishDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<VendorStudio> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(TymishDbContext).Assembly);
        }
    }
}