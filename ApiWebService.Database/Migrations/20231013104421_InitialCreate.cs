using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiWebService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_Persons", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Notes", x => x.Id));

            // Create 10 persons
            for (int i = 0; i < 10; i++)
            {
                var personId = Guid.NewGuid();

                migrationBuilder.InsertData(
                    table: "Persons",
                    columns: new[] { "Id", "FirstName", "LastName" },
                    values: new object[] { personId, $"FirstName {i}", $"LastName {i}" });

                // Create 10 notes for each person
                for (int j = 0; j < 10; j++)
                {
                    migrationBuilder.InsertData(
                        table: "Notes",
                        columns: new[] { "Id", "Title", "Content", "PersonId" },
                        values: new object[] { Guid.NewGuid(), $"Title {j}", $"Note {j} for Person {i}", personId });
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Notes");
            migrationBuilder.DropTable(name: "Persons");
        }
    }
}