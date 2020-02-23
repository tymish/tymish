using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tymish.Domain.Entities;

namespace Tymish.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("EmployeeId");
            
            builder.Property(e => e.GivenName)
                .IsRequired();
            
            builder.Property(e => e.FamilyName)
                .IsRequired();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(320);
            
            builder.Property(e => e.HourlyPay)
                .IsRequired();
        }
    }
}