namespace TallerMecanico.Dtos
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

        // Relación con Clientes
        public ICollection<ClienteDto> Clientes { get; set; }

        // Relación con Vehiculos
        public ICollection<VehiculoDto> Vehiculos { get; set; }

        // Auditoría
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}