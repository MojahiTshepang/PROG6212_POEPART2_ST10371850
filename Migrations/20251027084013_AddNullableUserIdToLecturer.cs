using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableUserIdToLecturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Lecturers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "LecturerID",
                keyValue: 1,
                column: "UserID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "LecturerID",
                keyValue: 2,
                column: "UserID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "LecturerID",
                keyValue: 3,
                column: "UserID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_UserID",
                table: "Lecturers",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Users_UserID",
                table: "Lecturers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Users_UserID",
                table: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_UserID",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Lecturers");
        }
    }
}
