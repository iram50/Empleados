using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Area
    {
        public Area()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int IdAreas { get; set; }
        public string DescripcionArea { get; set; } = null!;

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
