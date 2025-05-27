namespace CFE.Models
{
    public class Rol
    {
        public int Id_Rol { get; set; }
        public string? Nombre_Rol { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }

}
