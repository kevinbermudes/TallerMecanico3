namespace TallerMecanico.models;

public class Factura
{
    public int Id { get; set; }
    public string CodigoFactura { get; set; } 
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public decimal Total { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public EstadoFactura Estado { get; set; }

    public ICollection<CartaPago> CartasPago { get; set; }

    // Auditoría
    public DateTime FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}

public enum EstadoFactura
{
    Pagada,
    Pendiente
}
