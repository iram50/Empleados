using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CFE.Models;

namespace CFE.ViewModels
{
    public class GrupoConEmpleadoViewModel
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }  // ← adicional para mostrar nombre en el frontend

        public int IdCurso { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Horario { get; set; }
        public string Lugar { get; set; }
        public string? Comentarios { get; set; }
        public int? IdInstructor { get; set; }
        public string AreaOfrece { get; set; }
        public string Status { get; set; }
        public int Calificacion { get; set; } = 0;
    }
}


