using CFE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFE.Models
{
    public class Grupo
    {
        public Grupo()
        {
            Empleadocursos = new HashSet<Empleadocurso>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClaveGrupo { get; set; }
        [Required]
        public int? IdEmpleado { get; set; }
        [Required]
        public int IdCurso { get; set; }
        [Required]
        public DateTime FechaInicial { get; set; }

        [Required]
        public DateTime FechaFinal { get; set; }
        [Required]
        public string Horario { get; set; }
        [Required]
        public string Lugar { get; set; }

        public string? Comentarios { get; set; }

        public int? IdInstructor { get; set; }
        [Required]
        public string AreaOfrece { get; set; }
        [Required]
        public string Status { get; set; }
        public int Calificacion { get; set; } = 0;

        [ForeignKey(nameof(IdCurso))]
        public virtual Curso IdCursoNavigation { get; set; }

        [ForeignKey(nameof(IdInstructor))]
        public virtual Instructor IdInstructorNavigation { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado IdEmpleadoNavigation { get; set; }

        public virtual ICollection<Empleadocurso> Empleadocursos { get; set; }
    }
}

