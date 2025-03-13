using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Empleadocurso
    {
        public int IdEmpleado { get; set; }
        public int ClaveGrupo { get; set; }
        public string? EstatusCurso { get; set; }

        public virtual Grupo ClaveGrupoNavigation { get; set; } = null!;
        public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
    }
}
