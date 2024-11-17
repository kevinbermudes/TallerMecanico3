namespace TallerMecanico.models;

public class Cliente
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    // Datos personales
    public string PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string? EmailSecundario { get; set; }
    public string? Dni { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public Genero? Genero { get; set; }
    public EstadoCivil? EstadoCivil { get; set; }
    public string? Ocupacion { get; set; }
    public string? Notas { get; set; }

    // Relación con otras entidades
    public ICollection<Vehiculo> Vehiculos { get; set; }
    public ICollection<Servicio> Servicios { get; set; } 
    public ICollection<Factura> Facturas { get; set; }
    public ICollection<Pago> Pagos { get; set; }
    public ICollection<CartaPago> CartasPago { get; set; } 
    public ICollection<Notificacion> Notificaciones { get; set; } 
    public ICollection<Carrito> Carritos { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}

// Enum para Género
public enum Genero
{
    Masculino,
    Femenino,
    Otro
}

// Enum para Estado Civil
public enum EstadoCivil
{
    Soltero,
    Casado,
    Divorciado,
    Viudo,
    UnionLibre
}