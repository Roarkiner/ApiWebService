using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWebService.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create 10 persons
            for (int i = 0; i < 10; i++)
            {
                var personId = Guid.NewGuid();

                migrationBuilder.InsertData(
                    table: "Persons",
                    columns: new[] { "Id", "Email", "Password", "IsDeleted" },
                    values: new object[] { personId, $"Email {i}", $"Password {i}", false });

                // Create 10 notes for each person
                for (int j = 0; j < 10; j++)
                {
                    migrationBuilder.InsertData(
                        table: "Notes",
                        columns: new[] { "Id", "Title", "Content", "PersonId", "IsDeleted" },
                        values: new object[] { Guid.NewGuid(), $"Title {j}", $"Note {j} for Person {i}", personId, false });
                }
            }

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PersonId",
                table: "Notes",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
