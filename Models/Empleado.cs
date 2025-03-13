using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Empleado
    {
        public Empleado()
        {
            Empleadocursos = new HashSet<Empleadocurso>();
        }

        public int IdEmpleado { get; set; }
        public string Rfc { get; set; } = null!;
        public string Curp { get; set; } = null!;
        public string Nss { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string? ApellidoMaterno { get; set; }
        public string Nombre { get; set; } = null!;
        public int? IdArea { get; set; }
        public string? AreaEmpleado { get; set; }
        public int? IdPuesto { get; set; }
        public string? Puesto { get; set; }
        public string? Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public byte[]? Foto { get; set; }
        public bool? EmpleadoActivo { get; set; }

        public virtual Area? IdAreaNavigation { get; set; }
        public virtual Puesto? IdPuestoNavigation { get; set; }
        public virtual ICollection<Empleadocurso> Empleadocursos { get; set; }
    }
}
