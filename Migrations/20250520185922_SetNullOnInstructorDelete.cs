using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFE.Migrations
{
    public partial class SetNullOnInstructorDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos");

            migrationBuilder.DropColumn(
                name: "Instructor",
                table: "grupo");

            migrationBuilder.RenameColumn(
                name: "Fechainicial",
                table: "grupo",
                newName: "FechaInicial");

            migrationBuilder.UpdateData(
                table: "grupo",
                keyColumn: "Status",
                keyValue: null,
                column: "Status",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "grupo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.UpdateData(
                table: "grupo",
                keyColumn: "Lugar",
                keyValue: null,
                column: "Lugar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Lugar",
                table: "grupo",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Curso",
                table: "grupo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "grupo",
                keyColumn: "Horario",
                keyValue: null,
                column: "Horario",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Horario",
                table: "grupo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicial",
                table: "grupo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFinal",
                table: "grupo",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "grupo",
                keyColumn: "AreaOfrece",
                keyValue: null,
                column: "AreaOfrece",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AreaOfrece",
                table: "grupo",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<int>(
                name: "Calificacion",
                table: "grupo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpleado",
                table: "grupo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdInstructor",
                table: "grupo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "comprobante_escolaridad",
                table: "empleados",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "escolaridad",
                table: "empleados",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_nacimiento",
                table: "empleados",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ingreso_cfe",
                table: "empleados",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "jefe_inmediato",
                table: "empleados",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "residencia_especialidad",
                table: "empleados",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "rpe",
                table: "empleados",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "tipo_contrato",
                table: "empleados",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Calificacion",
                table: "empleadocursos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id_Instructor",
                table: "cursos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_grupo_IdEmpleado",
                table: "grupo",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_grupo_IdInstructor",
                table: "grupo",
                column: "IdInstructor");

            migrationBuilder.AddForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos",
                column: "Id_Instructor",
                principalTable: "instructor",
                principalColumn: "Id_Instructor");

            migrationBuilder.AddForeignKey(
                name: "FK_grupo_empleados_IdEmpleado",
                table: "grupo",
                column: "IdEmpleado",
                principalTable: "empleados",
                principalColumn: "Id_empleado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupo_instructor",
                table: "grupo",
                column: "IdInstructor",
                principalTable: "instructor",
                principalColumn: "Id_Instructor",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos");

            migrationBuilder.DropForeignKey(
                name: "FK_grupo_empleados_IdEmpleado",
                table: "grupo");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupo_instructor",
                table: "grupo");

            migrationBuilder.DropIndex(
                name: "IX_grupo_IdEmpleado",
                table: "grupo");

            migrationBuilder.DropIndex(
                name: "IX_grupo_IdInstructor",
                table: "grupo");

            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "grupo");

            migrationBuilder.DropColumn(
                name: "IdEmpleado",
                table: "grupo");

            migrationBuilder.DropColumn(
                name: "IdInstructor",
                table: "grupo");

            migrationBuilder.DropColumn(
                name: "comprobante_escolaridad",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "escolaridad",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "fecha_nacimiento",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "ingreso_cfe",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "jefe_inmediato",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "residencia_especialidad",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "rpe",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "tipo_contrato",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "empleadocursos");

            migrationBuilder.RenameColumn(
                name: "FechaInicial",
                table: "grupo",
                newName: "Fechainicial");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "grupo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Lugar",
                table: "grupo",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Curso",
                table: "grupo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Horario",
                table: "grupo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Fechainicial",
                table: "grupo",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaFinal",
                table: "grupo",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "AreaOfrece",
                table: "grupo",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<string>(
                name: "Instructor",
                table: "grupo",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Instructor",
                table: "cursos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "cursos_ibfk_1",
                table: "cursos",
                column: "Id_Instructor",
                principalTable: "instructor",
                principalColumn: "Id_Instructor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
