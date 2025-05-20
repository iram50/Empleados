using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFE.Migrations
{
    public partial class MakeInstructorOptional2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos");

            migrationBuilder.AddForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos",
                column: "Id_Instructor",
                principalTable: "instructor",
                principalColumn: "Id_Instructor",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos");

            migrationBuilder.AddForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos",
                column: "Id_Instructor",
                principalTable: "instructor",
                principalColumn: "Id_Instructor");
        }
    }
}
