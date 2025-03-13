using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Grupo
    {
        public Grupo()
        {
            Empleadocursos = new HashSet<Empleadocurso>();
        }

        public int ClaveGrupo { get; set; }
        public int? IdCurso { get; set; }
        public DateOnly? Fechainicial { get; set; }
        public DateOnly? FechaFinal { get; set; }
        public string? Horario { get; set; }
        public string? Lugar { get; set; }
        public string? Comentarios { get; set; }
        public string? Instructor { get; set; }
        public string? AreaOfrece { get; set; }
        public string? Status { get; set; }

        public virtual Curso? IdCursoNavigation { get; set; }
        public virtual ICollection<Empleadocurso> Empleadocursos { get; set; }
    }
}
