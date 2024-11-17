namespace TallerMecanico.Dtos
{
    public class ParteDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relación con Cliente
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }  

        // Auditoria
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}