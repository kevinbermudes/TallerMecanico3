namespace TallerMecanico.Dtos;

public class ProductoFacturaDto
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}