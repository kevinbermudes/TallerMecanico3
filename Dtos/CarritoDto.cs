namespace TallerMecanico.Dtos
{
    public class CarritoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }
        public ProductoDto Producto { get; set; } // Producto relacionado (si existe)
        public ServicioDto Servicio { get; set; } // Servicio relacionado (si existe)
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; }
    }
}