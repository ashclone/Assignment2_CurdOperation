using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment2_CurdOperation.Migrations
{
    public partial class AddViewModal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentHobbies",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    HobbyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentHobbies", x => new { x.StudentId, x.HobbyId });
                    table.ForeignKey(
                        name: "FK_StudentHobbies_Hobbies_HobbyId",
                        column: x => x.HobbyId,
                        principalTable: "Hobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentHobbies_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Hobbies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "Reading" },
                    { 8, "Playing" },
                    { 9, "Writting" },
                    { 10, "Cooking" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentHobbies_HobbyId",
                table: "StudentHobbies",
                column: "HobbyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentHobbies");

            migrationBuilder.DeleteData(
                table: "Hobbies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Hobbies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Hobbies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Hobbies",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
