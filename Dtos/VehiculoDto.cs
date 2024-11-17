namespace TallerMecanico.Dtos
{
    public class VehiculoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }  
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public string Placa { get; set; }

        // Relacion con los servicios realizados al vehículo
        public ICollection<ServicioDto> ServiciosRealizados { get; set; }

        // Auditoria
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}