namespace TallerMecanico.models;

public class Servicio
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public string Imagen { get;set; }
    // Relación muchos a muchos con Clientes
    public ICollection<ClienteServicio> ClienteServicios { get; set; } = new List<ClienteServicio>();

    public ICollection<Vehiculo> Vehiculos { get; set; }
    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}
