namespace TallerMecanico.Dtos
{
    public class CarritoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }  
        public int ProductoId { get; set; }
        public ProductoDto Producto { get; set; } 
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }

        // Auditoria
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}