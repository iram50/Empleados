﻿// <auto-generated />
using System;
using CFE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CFE.Migrations
{
    [DbContext(typeof(empresaContext))]
    [Migration("20250527181419_AddRolAndUsuario")]
    partial class AddRolAndUsuario
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "6.0.35")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("CFE.Models.Area", b =>
                {
                    b.Property<int>("IdAreas")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_areas");

                    b.Property<string>("DescripcionArea")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdAreas")
                        .HasName("PRIMARY");

                    b.ToTable("areas", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Curso", b =>
                {
                    b.Property<int>("IdCurso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Curso");

                    b.Property<int?>("Id_Instructor")
                        .HasColumnType("int")
                        .HasColumnName("Id_Instructor");

                    b.Property<string>("NombreCurso")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdCurso")
                        .HasName("PRIMARY");

                    b.HasIndex("Id_Instructor");

                    b.ToTable("cursos", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Empleado", b =>
                {
                    b.Property<int>("IdEmpleado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_empleado");

                    b.Property<string>("ApellidoMaterno")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ApellidoPaterno")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("AreaEmpleado")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CorreoElectronico")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Curp")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("varchar(18)")
                        .HasColumnName("CURP");

                    b.Property<bool?>("EmpleadoActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValueSql("'1'");

                    b.Property<byte[]>("Foto")
                        .HasColumnType("blob");

                    b.Property<int?>("IdArea")
                        .HasColumnType("int")
                        .HasColumnName("Id_area");

                    b.Property<int?>("IdPuesto")
                        .HasColumnType("int")
                        .HasColumnName("Id_puesto");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nss")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)")
                        .HasColumnName("NSS");

                    b.Property<string>("Puesto")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Rfc")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)")
                        .HasColumnName("RFC");

                    b.Property<string>("Telefono")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("comprobante_escolaridad")
                        .HasColumnType("longtext");

                    b.Property<string>("escolaridad")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("fecha_nacimiento")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ingreso_cfe")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("jefe_inmediato")
                        .HasColumnType("longtext");

                    b.Property<string>("residencia_especialidad")
                        .HasColumnType("longtext");

                    b.Property<string>("rpe")
                        .HasColumnType("longtext");

                    b.Property<string>("tipo_contrato")
                        .HasColumnType("longtext");

                    b.HasKey("IdEmpleado")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Curp" }, "CURP")
                        .IsUnique();

                    b.HasIndex(new[] { "IdArea" }, "Id_area");

                    b.HasIndex(new[] { "IdPuesto" }, "Id_puesto");

                    b.HasIndex(new[] { "Nss" }, "NSS")
                        .IsUnique();

                    b.HasIndex(new[] { "Rfc" }, "RFC")
                        .IsUnique();

                    b.ToTable("empleados", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Empleadocurso", b =>
                {
                    b.Property<int>("IdEmpleado")
                        .HasColumnType("int")
                        .HasColumnName("Id_Empleado");

                    b.Property<int>("ClaveGrupo")
                        .HasColumnType("int")
                        .HasColumnName("Clave_Grupo");

                    b.Property<int?>("Calificacion")
                        .HasColumnType("int");

                    b.Property<string>("EstatusCurso")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("IdEmpleado", "ClaveGrupo")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "ClaveGrupo" }, "Clave_Grupo");

                    b.ToTable("empleadocursos", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Grupo", b =>
                {
                    b.Property<int>("ClaveGrupo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Clave_Grupo");

                    b.Property<string>("AreaOfrece")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Calificacion")
                        .HasColumnType("int");

                    b.Property<string>("Comentarios")
                        .HasColumnType("text");

                    b.Property<DateTime>("FechaFinal")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicial")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Horario")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("IdCurso")
                        .HasColumnType("int")
                        .HasColumnName("Id_Curso");

                    b.Property<int?>("IdEmpleado")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdInstructor")
                        .HasColumnType("int")
                        .HasColumnName("IdInstructor");

                    b.Property<string>("Lugar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("ClaveGrupo")
                        .HasName("PRIMARY");

                    b.HasIndex("IdEmpleado");

                    b.HasIndex("IdInstructor");

                    b.HasIndex(new[] { "IdCurso" }, "Id_Curso");

                    b.ToTable("grupo", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Instructor", b =>
                {
                    b.Property<int>("Id_Instructor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_Instructor");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NombreInstructor")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id_Instructor")
                        .HasName("PRIMARY");

                    b.ToTable("instructor", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Puesto", b =>
                {
                    b.Property<int>("IdPuesto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_puesto");

                    b.Property<string>("DescripcionPuesto")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdPuesto")
                        .HasName("PRIMARY");

                    b.ToTable("puesto", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Usuario", b =>
                {
                    b.Property<int>("Id_usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id_usuario");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Id_rol")
                        .HasColumnType("int")
                        .HasColumnName("Id_rol");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("usuario")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id_usuario");

                    b.HasIndex("Id_rol");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("CFE.Models.Curso", b =>
                {
                    b.HasOne("CFE.Models.Instructor", "Instructor")
                        .WithMany("Cursos")
                        .HasForeignKey("Id_Instructor")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("cursos_ibfk_1");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("CFE.Models.Empleado", b =>
                {
                    b.HasOne("CFE.Models.Area", "IdAreaNavigation")
                        .WithMany("Empleados")
                        .HasForeignKey("IdArea")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("empleados_ibfk_1");

                    b.HasOne("CFE.Models.Puesto", "IdPuestoNavigation")
                        .WithMany("Empleados")
                        .HasForeignKey("IdPuesto")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("empleados_ibfk_2");

                    b.Navigation("IdAreaNavigation");

                    b.Navigation("IdPuestoNavigation");
                });

            modelBuilder.Entity("CFE.Models.Empleadocurso", b =>
                {
                    b.HasOne("CFE.Models.Grupo", "ClaveGrupoNavigation")
                        .WithMany("Empleadocursos")
                        .HasForeignKey("ClaveGrupo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("empleadocursos_ibfk_2");

                    b.HasOne("CFE.Models.Empleado", "IdEmpleadoNavigation")
                        .WithMany("Empleadocursos")
                        .HasForeignKey("IdEmpleado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("empleadocursos_ibfk_1");

                    b.Navigation("ClaveGrupoNavigation");

                    b.Navigation("IdEmpleadoNavigation");
                });

            modelBuilder.Entity("CFE.Models.Grupo", b =>
                {
                    b.HasOne("CFE.Models.Curso", "IdCursoNavigation")
                        .WithMany("Grupos")
                        .HasForeignKey("IdCurso")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("grupo_ibfk_1");

                    b.HasOne("CFE.Models.Empleado", "IdEmpleadoNavigation")
                        .WithMany()
                        .HasForeignKey("IdEmpleado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CFE.Models.Instructor", "IdInstructorNavigation")
                        .WithMany()
                        .HasForeignKey("IdInstructor")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Grupo_instructor");

                    b.Navigation("IdCursoNavigation");

                    b.Navigation("IdEmpleadoNavigation");

                    b.Navigation("IdInstructorNavigation");
                });

            modelBuilder.Entity("CFE.Models.Usuario", b =>
                {
                    b.HasOne("CFE.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("Id_rol")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Usuarios_Roles");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("CFE.Models.Area", b =>
                {
                    b.Navigation("Empleados");
                });

            modelBuilder.Entity("CFE.Models.Curso", b =>
                {
                    b.Navigation("Grupos");
                });

            modelBuilder.Entity("CFE.Models.Empleado", b =>
                {
                    b.Navigation("Empleadocursos");
                });

            modelBuilder.Entity("CFE.Models.Grupo", b =>
                {
                    b.Navigation("Empleadocursos");
                });

            modelBuilder.Entity("CFE.Models.Instructor", b =>
                {
                    b.Navigation("Cursos");
                });

            modelBuilder.Entity("CFE.Models.Puesto", b =>
                {
                    b.Navigation("Empleados");
                });

            modelBuilder.Entity("CFE.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
