using System;
using System.Collections.Generic;

namespace CFE.Models
{
    public partial class Instructor
    {
        public int IdInstructor { get; set; }
        public string NombreInstructor { get; set; } = null!;
        public string? Descripcion { get; set; }
    }
}
