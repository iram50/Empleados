using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Instructor
    {
        public Instructor()
        {
            Cursos = new HashSet<Curso>();
        }

        public int Id_Instructor { get; set; }
        public string NombreInstructor { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public virtual ICollection<Curso> Cursos { get; set; } = null!;
    }
}
