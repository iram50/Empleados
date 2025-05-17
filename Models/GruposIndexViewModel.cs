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

        // Grupo nuevo para registrar
        public Grupo NuevoGrupo { get; set; }
    }
}

