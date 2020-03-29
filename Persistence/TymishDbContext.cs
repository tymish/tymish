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

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeReport> TimeReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(TymishDbContext).Assembly);


            // Dev Seed data
            var alice = new Employee
            {
                Id = Guid.Parse("9c86216f-6422-47ba-b158-bdac76805c0a"),
                EmployeeNumber = 1,
                GivenName = "Alice",
                FamilyName = "Zuberg",
                Email = "alice.zuberg@gmail.com",
                HourlyPay = 25
            };
            var bob = new Employee
            {
                Id = Guid.Parse("3f289a60-366f-4316-b6f2-e68e811f8b05"),
                EmployeeNumber = 2,
                GivenName = "Bob",
                FamilyName = "McPhearson",
                Email = "bob.mcphearson@gmail.com",
                HourlyPay = 20
            };
            builder.Entity<Employee>().HasData(alice);
            builder.Entity<Employee>().HasData(bob);

            // Alice time reports
            builder.Entity<TimeReport>().HasData(new TimeReport
            {
                Id = Guid.Parse("5d6e0332-f791-4dad-bb02-269d56b1df57"),
                Sent = new DateTime(2019, 12, 25),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                EmployeeId = alice.Id
            });

            builder.Entity<TimeReport>().HasData(new TimeReport
            {
                Id = Guid.Parse("0470ff9a-f359-40a4-a5de-cbbd765c8e7b"),
                Sent = new DateTime(2020, 1, 25),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                EmployeeId = alice.Id
            });

            builder.Entity<TimeReport>().HasData(new TimeReport
            {
                Id = Guid.Parse("dfa95a83-3187-4ffb-a6f9-5a8a62d6bf9c"),
                Sent = new DateTime(2020, 2, 25),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                EmployeeId = alice.Id
            });

            var dec = new TimeEntry[]
            {
                new TimeEntry
                { 
                    Start = new DateTime(2019, 12, 10, 5, 0, 0),
                    End = new DateTime(2019, 12, 10, 6, 0, 0),
                    Comments = string.Empty
                },
                new TimeEntry
                { 
                    Start = new DateTime(2019, 12, 17, 5, 0, 0),
                    End = new DateTime(2019, 12, 17, 6, 0, 0),
                    Comments = string.Empty
                },
                new TimeEntry
                { 
                    Start = new DateTime(2019, 12, 24, 5, 0, 0),
                    End = new DateTime(2019, 12, 24, 6, 0, 0),
                    Comments = string.Empty
                }
            };
            var jan = new TimeEntry[] 
            {
                new TimeEntry
                { 
                    Start = new DateTime(2020, 1, 10, 5, 0, 0),
                    End = new DateTime(2020, 1, 10, 6, 0, 0),
                    Comments = string.Empty
                },
                new TimeEntry
                { 
                    Start = new DateTime(2020, 1, 17, 5, 0, 0),
                    End = new DateTime(2020, 1, 17, 6, 0, 0),
                    Comments = string.Empty
                },
                new TimeEntry
                { 
                    Start = new DateTime(2020, 1, 24, 5, 0, 0),
                    End = new DateTime(2020, 1, 24, 6, 0, 0),
                    Comments = string.Empty
                }
            };
            var feb = new TimeEntry[]
            {
                new TimeEntry()
                { 
                    Start = new DateTime(2020, 2, 10, 5, 0, 0),
                    End = new DateTime(2020, 2, 10, 6, 0, 0),
                    Comments = string.Empty
                },
                new TimeEntry()
                { 
                    Start = new DateTime(2020, 2, 17, 5, 0, 0),
                    End = new DateTime(2020, 2, 17, 6, 0, 0),
                    Comments = string.Empty
                },
                new TimeEntry()
                { 
                    Start = new DateTime(2020, 2, 24, 5, 0, 0),
                    End = new DateTime(2020, 2, 24, 6, 0, 0),
                    Comments = string.Empty
                }
            };
            
            //Bob time reports
            builder.Entity<TimeReport>().HasData(new
            {
                Id = Guid.Parse("d9e353ca-a2ae-4b86-a60c-07ea19d2e689"),
                Sent = new DateTime(2019, 12, 25),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                EmployeeId = bob.Id
            });
            builder.Entity<TimeReport>().HasData(new
            {
                Id = Guid.Parse("28a4410c-710c-4b2e-a950-67d74ebebd87"),
                Sent = new DateTime(2020, 1, 25),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                EmployeeId = bob.Id
            });
            builder.Entity<TimeReport>().HasData(new
            {
                Id = Guid.Parse("2423ed81-d924-46d9-a44a-74ff3973ea3e"),
                Sent = new DateTime(2020, 2, 25),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                EmployeeId = bob.Id
            });
        }
    }
}