using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tymish.Domain.Entities;

namespace Tymish.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasAlternateKey(e => e.EmployeeNumber);

            builder.Property(e => e.EmployeeNumber)
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(e => e.GivenName)
                .IsRequired();
            
            builder.Property(e => e.FamilyName)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(320)
                .IsRequired();
            
            builder.Property(e => e.HourlyPay)
                .HasColumnType("money")
                .IsRequired();
        }
    }
}