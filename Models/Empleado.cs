using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? tipo_contrato { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public DateTime? ingreso_cfe { get; set; }

        public string? rpe { get; set; }

        public string? jefe_inmediato { get; set; }

        public string? escolaridad { get; set; }

        public string? comprobante_escolaridad { get; set; }
        public bool? EmpleadoActivo { get; set; }

        public string? residencia_especialidad { get; set; }


        public virtual Area? IdAreaNavigation { get; set; }
        public virtual Puesto? IdPuestoNavigation { get; set; }
        public virtual ICollection<Empleadocurso> Empleadocursos { get; set; }

        [NotMapped]
        public int? Edad
        {
            get
            {
                if (fecha_nacimiento == null)
                    return null;

                var hoy = DateTime.Today;
                var edad = hoy.Year - fecha_nacimiento.Value.Year;
                if (fecha_nacimiento.Value.Date > hoy.AddYears(-edad)) edad--;
                return edad;
            }
        }

    }
}
