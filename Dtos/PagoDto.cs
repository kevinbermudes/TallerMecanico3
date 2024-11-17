using TallerMecanico.models;

namespace TallerMecanico.Dtos
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public TipoPago TipoPago { get; set; }
        public int? ProductoId { get; set; }   
        public int? ServicioId { get; set; }   
        public int? ParteId { get; set; }      
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }

}