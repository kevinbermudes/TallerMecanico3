using TallerMecanico.models;

namespace TallerMecanico.Dtos
{
    public class FacturaDto
    {
        public int Id { get; set; }
        public string CodigoFactura { get; set; }
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }  
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public EstadoFactura Estado { get; set; }
        
        // Relacion con Cartas de Pago
        public ICollection<CartaPagoDto> CartasPago { get; set; }

        // Auditoria
        public DateTime FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}