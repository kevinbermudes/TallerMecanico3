namespace TallerMecanico.models;

public class Notificacion
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public string Mensaje { get; set; }
    public DateTime FechaEnvio { get; set; }
    public TipoNotificacion Tipo { get; set; }
    public bool Leido { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}

public enum TipoNotificacion
{
    General,
    Factura,
    Carrito,
    Servicio,
    NuevaFactura,
    RecordatorioPago
}
