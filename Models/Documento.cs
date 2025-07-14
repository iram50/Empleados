using CFE.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuProyecto.Models
{
    public class Documento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [ForeignKey("EmpleadoId")]
        public Empleado Empleado { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Required]
        public string Tipo { get; set; }

        public byte[] Archivo { get; set; }

        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
}
