namespace TallerMecanico.Dtos
{
    public class VehiculoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; } // Este es suficiente para asociar el cliente
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Year { get; set; }
        public string Placa { get; set; }
        public List<int>? ServiciosRealizadosIds { get; set; }

        // Auditoria
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}