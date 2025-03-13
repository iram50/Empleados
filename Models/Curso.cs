using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Curso
    {
        public Curso()
        {
            Grupos = new HashSet<Grupo>();
        }

        public int IdCurso { get; set; }
        public string NombreCurso { get; set; } = null!;
        public string NombreInstructor { get; set; } = null!;

        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}
