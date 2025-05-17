using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFE.Models
{
    public partial class Instructor
    {
        public Instructor()
        {
            Cursos = new HashSet<Curso>();
        }

        [Column("Id_Instructor")]
        public int Id_Instructor { get; set; }
        public string NombreInstructor { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public virtual ICollection<Curso> Cursos { get; set; } = null!;


    }
}
