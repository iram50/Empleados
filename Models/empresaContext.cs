using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CFE.Models
{
    public partial class empresaContext : DbContext
    {
        public empresaContext()
        {
        }

        public empresaContext(DbContextOptions<empresaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Curso> Cursos { get; set; } = null!;
        public virtual DbSet<Empleado> Empleados { get; set; } = null!;
        public virtual DbSet<Empleadocurso> Empleadocursos { get; set; } = null!;
        public virtual DbSet<Grupo> Grupos { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<Puesto> Puestos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseMySql("server=localhost;port=3306;database=empresa;uid=root;password=papajose", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.2.0-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.IdAreas)
                    .HasName("PRIMARY");

                entity.ToTable("areas");

                entity.Property(e => e.IdAreas).HasColumnName("Id_areas");

                entity.Property(e => e.DescripcionArea).HasMaxLength(255);
            });

            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(e => e.IdCurso)
                    .HasName("PRIMARY");

                entity.ToTable("cursos");

                entity.Property(e => e.IdCurso).HasColumnName("Id_Curso");

                entity.Property(e => e.NombreCurso).HasMaxLength(255);

                entity.Property(e => e.Id_Instructor)
                    .HasColumnName("Id_Instructor")
                    .IsRequired(false);

                entity.HasOne(e => e.Instructor)
                    .WithMany(e => e.Cursos)
                    .HasForeignKey(e => e.Id_Instructor)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("cursos_ibfk_1");
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.IdEmpleado)
                    .HasName("PRIMARY");

                entity.ToTable("empleados");

                entity.HasIndex(e => e.Curp, "CURP")
                    .IsUnique();

                entity.HasIndex(e => e.IdArea, "Id_area");

                entity.HasIndex(e => e.IdPuesto, "Id_puesto");

                entity.HasIndex(e => e.Nss, "NSS")
                    .IsUnique();

                entity.HasIndex(e => e.Rfc, "RFC")
                    .IsUnique();

                entity.Property(e => e.IdEmpleado).HasColumnName("Id_empleado");

                entity.Property(e => e.ApellidoMaterno).HasMaxLength(50);

                entity.Property(e => e.ApellidoPaterno).HasMaxLength(50);

                entity.Property(e => e.AreaEmpleado).HasMaxLength(255);

                entity.Property(e => e.CorreoElectronico).HasMaxLength(100);

                entity.Property(e => e.Curp)
                    .HasMaxLength(18)
                    .HasColumnName("CURP");

                entity.Property(e => e.EmpleadoActivo).HasDefaultValueSql("'1'");

                entity.Property(e => e.Foto).HasColumnType("blob");

                entity.Property(e => e.IdArea).HasColumnName("Id_area");

                entity.Property(e => e.IdPuesto).HasColumnName("Id_puesto");

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Nss)
                    .HasMaxLength(11)
                    .HasColumnName("NSS");

                entity.Property(e => e.Puesto).HasMaxLength(255);

                entity.Property(e => e.Rfc)
                    .HasMaxLength(13)
                    .HasColumnName("RFC");

                entity.Property(e => e.Telefono).HasMaxLength(15);

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("empleados_ibfk_1");

                entity.HasOne(d => d.IdPuestoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.IdPuesto)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("empleados_ibfk_2");
            });

            modelBuilder.Entity<Empleadocurso>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.ClaveGrupo })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("empleadocursos");

                entity.HasIndex(e => e.ClaveGrupo, "Clave_Grupo");

                entity.Property(e => e.IdEmpleado).HasColumnName("Id_Empleado");

                entity.Property(e => e.ClaveGrupo).HasColumnName("Clave_Grupo");

                entity.Property(e => e.EstatusCurso).HasMaxLength(50);

                entity.HasOne(d => d.ClaveGrupoNavigation)
                    .WithMany(p => p.Empleadocursos)
                    .HasForeignKey(d => d.ClaveGrupo)
                    .HasConstraintName("empleadocursos_ibfk_2");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Empleadocursos)
                    .HasForeignKey(d => d.IdEmpleado)
                    .HasConstraintName("empleadocursos_ibfk_1");
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.HasKey(e => e.ClaveGrupo)
                    .HasName("PRIMARY");

                entity.ToTable("grupo");

                entity.HasIndex(e => e.IdCurso, "Id_Curso");

                entity.Property(e => e.ClaveGrupo).HasColumnName("Clave_Grupo");

                entity.Property(e => e.AreaOfrece).HasMaxLength(255);

                entity.Property(e => e.Comentarios).HasColumnType("text");

                entity.Property(e => e.Horario).HasMaxLength(50);

                entity.Property(e => e.IdCurso).HasColumnName("Id_Curso");

                entity.Property(e => e.IdInstructor)
                        .HasColumnName("IdInstructor")
                        .IsRequired(false);
                entity.HasOne(d => d.IdInstructorNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdInstructor)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Grupo_instructor");

                entity.Property(e => e.Lugar).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.IdCursoNavigation)
                    .WithMany(p => p.Grupos)
                    .HasForeignKey(d => d.IdCurso)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("grupo_ibfk_1");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(e => e.Id_Instructor)
                    .HasName("PRIMARY");

                entity.ToTable("instructor");

                entity.Property(e => e.Id_Instructor).HasColumnName("Id_Instructor");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.NombreInstructor).HasMaxLength(255);
            });

            modelBuilder.Entity<Puesto>(entity =>
            {
                entity.HasKey(e => e.IdPuesto)
                    .HasName("PRIMARY");

                entity.ToTable("puesto");

                entity.Property(e => e.IdPuesto).HasColumnName("Id_puesto");

                entity.Property(e => e.DescripcionPuesto).HasMaxLength(255);
            });


            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Roles");

                entity.HasKey(e => e.Id_Rol);

                entity.Property(e => e.Id_Rol)
                    .HasColumnName("Id_Rol");

                entity.Property(e => e.Nombre_Rol)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasKey(e => e.Id_usuario);

                entity.Property(e => e.Id_usuario)
                    .HasColumnName("Id_usuario");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UsuarioNombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RolId)
                    .HasColumnName("RolId");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Id_usuario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Usuarios_Roles");
            });



            OnModelCreatingPartial(modelBuilder);
        }




        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
