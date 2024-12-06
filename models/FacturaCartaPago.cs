namespace TallerMecanico.models;

public class FacturaCartaPago
{
    public int FacturaId { get; set; }
    public Factura Factura { get; set; }
    public int CartaPagoId { get; set; }
    public CartaPago CartaPago { get; set; }
    public DateTime FechaAsociacion { get; set; } = DateTime.UtcNow;
}
