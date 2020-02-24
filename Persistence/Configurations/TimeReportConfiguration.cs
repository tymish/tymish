using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tymish.Domain.Entities;

namespace Tymish.Persistence.Configurations
{
    public class TimeReportConfiguration : IEntityTypeConfiguration<TimeReport>
    {
        public void Configure(EntityTypeBuilder<TimeReport> builder)
        {
            builder.Property(e => e.TimeEntries)
                .HasColumnType("jsonb");
        }
    }
}