using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFE.Migrations
{
    public partial class FixCursoInstructorRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "areas",
                columns: table => new
                {
                    Id_areas = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DescripcionArea = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_areas);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "instructor",
                columns: table => new
                {
                    Id_Instructor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreInstructor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_Instructor);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "puesto",
                columns: table => new
                {
                    Id_puesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DescripcionPuesto = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_puesto);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "cursos",
                columns: table => new
                {
                    Id_Curso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreCurso = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_Instructor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_Curso);
                    table.ForeignKey(
                        name: "cursos_ibfk_1",
                        column: x => x.Id_Instructor,
                        principalTable: "instructor",
                        principalColumn: "Id_Instructor",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "empleados",
                columns: table => new
                {
                    Id_empleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RFC = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CURP = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NSS = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoPaterno = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApellidoMaterno = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_area = table.Column<int>(type: "int", nullable: true),
                    AreaEmpleado = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_puesto = table.Column<int>(type: "int", nullable: true),
                    Puesto = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CorreoElectronico = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<byte[]>(type: "blob", nullable: true),
                    EmpleadoActivo = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id_empleado);
                    table.ForeignKey(
                        name: "empleados_ibfk_1",
                        column: x => x.Id_area,
                        principalTable: "areas",
                        principalColumn: "Id_areas",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "empleados_ibfk_2",
                        column: x => x.Id_puesto,
                        principalTable: "puesto",
                        principalColumn: "Id_puesto",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "grupo",
                columns: table => new
                {
                    Clave_Grupo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Curso = table.Column<int>(type: "int", nullable: true),
                    Fechainicial = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaFinal = table.Column<DateOnly>(type: "date", nullable: true),
                    Horario = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lugar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comentarios = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Instructor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AreaOfrece = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Clave_Grupo);
                    table.ForeignKey(
                        name: "grupo_ibfk_1",
                        column: x => x.Id_Curso,
                        principalTable: "cursos",
                        principalColumn: "Id_Curso",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "empleadocursos",
                columns: table => new
                {
                    Id_Empleado = table.Column<int>(type: "int", nullable: false),
                    Clave_Grupo = table.Column<int>(type: "int", nullable: false),
                    EstatusCurso = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id_Empleado, x.Clave_Grupo })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "empleadocursos_ibfk_1",
                        column: x => x.Id_Empleado,
                        principalTable: "empleados",
                        principalColumn: "Id_empleado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "empleadocursos_ibfk_2",
                        column: x => x.Clave_Grupo,
                        principalTable: "grupo",
                        principalColumn: "Clave_Grupo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_Id_Instructor",
                table: "cursos",
                column: "Id_Instructor");

            migrationBuilder.CreateIndex(
                name: "Clave_Grupo",
                table: "empleadocursos",
                column: "Clave_Grupo");

            migrationBuilder.CreateIndex(
                name: "CURP",
                table: "empleados",
                column: "CURP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Id_area",
                table: "empleados",
                column: "Id_area");

            migrationBuilder.CreateIndex(
                name: "Id_puesto",
                table: "empleados",
                column: "Id_puesto");

            migrationBuilder.CreateIndex(
                name: "NSS",
                table: "empleados",
                column: "NSS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RFC",
                table: "empleados",
                column: "RFC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Id_Curso",
                table: "grupo",
                column: "Id_Curso");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "empleadocursos");

            migrationBuilder.DropTable(
                name: "empleados");

            migrationBuilder.DropTable(
                name: "grupo");

            migrationBuilder.DropTable(
                name: "areas");

            migrationBuilder.DropTable(
                name: "puesto");

            migrationBuilder.DropTable(
                name: "cursos");

            migrationBuilder.DropTable(
                name: "instructor");
        }
    }
}
