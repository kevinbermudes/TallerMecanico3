namespace TallerMecanico.models;

public class CartaPago
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public Factura Factura { get; set; }
    public int ClienteId { get; set; } 
    public Cliente Cliente { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public MetodoPago MetodoPago { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}


public enum MetodoPago
{
    Efectivo,
    Tarjeta,
    Transferencia,
    Pasarela
}
