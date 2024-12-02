namespace TallerMecanico.models;

public class ClienteServicio
{
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public int ServicioId { get; set; }
    public Servicio Servicio { get; set; }

    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
}
