namespace TallerMecanico.models;

public class Servicio
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }

    // Relación muchos a muchos con Clientes
    public ICollection<Cliente> Clientes { get; set; } 
    public ICollection<Vehiculo> Vehiculos { get; set; }
    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}
