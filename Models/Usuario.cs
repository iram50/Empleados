namespace CFE.Models;

public class Usuario
{
    public int Id_usuario { get; set; }  
    public string? Nombre { get; set; }
    public string? UsuarioNombre { get; set; } 
    public string? Clave { get; set; }

    public int RolId { get; set; } 
    public Rol? Rol { get; set; }
}
