namespace TallerMecanico.models;

public class ServicioFactura
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public Factura Factura { get; set; }
    public int ServicioId { get; set; }
    public Servicio Servicio { get; set; }
    public decimal Precio { get; set; }
}