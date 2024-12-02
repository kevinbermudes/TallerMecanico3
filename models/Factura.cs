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
    public string Comentarios { get; set; } // Nuevo campo

    public ICollection<CartaPago> CartasPago { get; set; }
    public ICollection<ProductoFactura> ProductosFactura { get; set; }

    // Relación con Servicios
    public ICollection<ServicioFactura> ServiciosFactura { get; set; }

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
