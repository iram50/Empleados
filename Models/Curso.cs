using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int Id_Instructor { get; set; }
        public virtual Instructor Instructor { get; set; } = null!;
        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}
