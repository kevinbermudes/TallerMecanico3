namespace TallerMecanico.models;

public class Carrito
{
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public int? ProductoId { get; set; } // Opcional
    public Producto Producto { get; set; }

    public int? ServicioId { get; set; } // Opcional
    public Servicio Servicio { get; set; }

    public int? ParteId { get; set; } // Opcional
    public Parte Parte { get; set; }

    public int Cantidad { get; set; }
    public decimal PrecioTotal { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}