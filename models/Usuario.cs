namespace TallerMecanico.models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public RolUsuario Rol { get; set; } // Admin, Cliente
    public DateTime FechaRegistro { get; set; }
    public bool Estado { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; } 
    public bool EstaBorrado { get; set; } = false;
}

public enum RolUsuario
{
    Admin,
    Cliente
}