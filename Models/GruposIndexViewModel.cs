using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CFE.Models;

namespace CFE.ViewModels
{
    public class GruposIndexViewModel
    {
        // Lista de todos los empleados
        public List<Empleado> Empleados { get; set; }

        // Id del empleado actualmente seleccionado
        public int? EmpleadoSeleccionadoId { get; set; }

        // Lista de cursos en los que el empleado está inscrito
        public List<Grupo> CursosInscritos { get; set; }

        // Cursos disponibles para inscripción
        public List<Curso> CursosDisponibles { get; set; }

        public Grupo NuevoGrupo { get; set; }

        // Grupo nuevo para registrar
        public List<Grupo> NuevosGrupos { get; set; } = new();

        public List<int> EmpleadosIds { get; set; } = new();

        public string NombreEmpleadoSeleccionado { get; set; }

    }
}

