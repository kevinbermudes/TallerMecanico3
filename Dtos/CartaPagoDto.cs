using TallerMecanico.models;

namespace TallerMecanico.Dtos
{
    public class CartaPagoDto
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public FacturaDto Factura { get; set; }  
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }  
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public MetodoPago MetodoPago { get; set; }

        // Auditoria
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}