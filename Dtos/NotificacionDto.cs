using TallerMecanico.models;

namespace TallerMecanico.Dtos
{
    public class NotificacionDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; } 
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public bool Leido { get; set; }

        // Auditoría
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}