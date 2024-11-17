using TallerMecanico.models;

namespace TallerMecanico.Dtos;

public class ClienteDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public UsuarioDto Usuario { get; set; }

    // Datos personales
    public string PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? EmailSecundario { get; set; }
    public string? Dni { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public Genero? Genero { get; set; }
    public EstadoCivil? EstadoCivil { get; set; }
    public string? Ocupacion { get; set; }
    public string? Notas { get; set; }

    // Relación con otras entidades
    public ICollection<VehiculoDto>? Vehiculos { get; set; }
    public ICollection<ServicioDto>? Servicios { get; set; }
    public ICollection<FacturaDto>? Facturas { get; set; }
    public ICollection<PagoDto>? Pagos { get; set; }
    public ICollection<CartaPagoDto>? CartasPago { get; set; }
    public ICollection<NotificacionDto>? Notificaciones { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool? EstaBorrado { get; set; } = false;
}