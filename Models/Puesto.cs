using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Puesto
    {
        public Puesto()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int IdPuesto { get; set; }
        public string DescripcionPuesto { get; set; } = null!;

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
