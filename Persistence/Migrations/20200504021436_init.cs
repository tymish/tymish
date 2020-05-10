using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tymish.Domain.Entities;

namespace Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeNumber = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GivenName = table.Column<string>(nullable: false),
                    FamilyName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(maxLength: 320, nullable: false),
                    HourlyPay = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.UniqueConstraint("AK_Employees_EmployeeNumber", x => x.EmployeeNumber);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PayPeriod = table.Column<DateTime>(nullable: false),
                    Sent = table.Column<DateTime>(nullable: true),
                    Submitted = table.Column<DateTime>(nullable: true),
                    Paid = table.Column<DateTime>(nullable: true),
                    TimeEntries = table.Column<IList<TimeEntry>>(type: "jsonb", nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "EmployeeNumber", "FamilyName", "GivenName", "HourlyPay" },
                values: new object[,]
                {
                    { new Guid("9c86216f-6422-47ba-b158-bdac76805c0a"), "alice.zuberg@gmail.com", 1, "Zuberg", "Alice", 25m },
                    { new Guid("3f289a60-366f-4316-b6f2-e68e811f8b05"), "bob.mcphearson@gmail.com", 2, "McPhearson", "Bob", 20m }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "EmployeeId", "Paid", "PayPeriod", "Sent", "Submitted", "TimeEntries" },
                values: new object[,]
                {
                    { new Guid("5d6e0332-f791-4dad-bb02-269d56b1df57"), new Guid("9c86216f-6422-47ba-b158-bdac76805c0a"), null, new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { new Guid("0470ff9a-f359-40a4-a5de-cbbd765c8e7b"), new Guid("9c86216f-6422-47ba-b158-bdac76805c0a"), null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { new Guid("dfa95a83-3187-4ffb-a6f9-5a8a62d6bf9c"), new Guid("9c86216f-6422-47ba-b158-bdac76805c0a"), null, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { new Guid("d9e353ca-a2ae-4b86-a60c-07ea19d2e689"), new Guid("3f289a60-366f-4316-b6f2-e68e811f8b05"), null, new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { new Guid("28a4410c-710c-4b2e-a950-67d74ebebd87"), new Guid("3f289a60-366f-4316-b6f2-e68e811f8b05"), null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { new Guid("2423ed81-d924-46d9-a44a-74ff3973ea3e"), new Guid("3f289a60-366f-4316-b6f2-e68e811f8b05"), null, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_EmployeeId",
                table: "Invoices",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
