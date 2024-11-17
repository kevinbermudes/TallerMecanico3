namespace TallerMecanico.models;

public class Parte
{
    public int Id { get; set; }
    public string Descripcion { get; set; }
    public decimal Costo { get; set; }
    public string imagen { get; set; }
    public DateTime FechaCreacion { get; set; }
    
    // Relación con Cliente
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    // Auditoría
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}
