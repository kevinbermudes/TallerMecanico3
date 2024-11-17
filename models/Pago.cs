namespace TallerMecanico.models;

public class Pago
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaTransaccion { get; set; }

    // Relación con el objeto pagado (Producto, Servicio, Parte)
    public TipoPago TipoPago { get; set; }
    public int? ProductoId { get; set; }   // Ahora es opcional
    public Producto Producto { get; set; }
    public int? ServicioId { get; set; }   // Ahora es opcional
    public Servicio Servicio { get; set; }
    public int? ParteId { get; set; }      // Ahora es opcional
    public Parte Parte { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}


public enum TipoPago
{
    Producto,
    Servicio,
    Parte
}
