using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.Dtos
{
    public class ServicioDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido.")]

        public string Nombre { get; set; }
        [Required(ErrorMessage = "La Descripción es requerida.")]

        public string Descripcion { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El Precio debe ser mayor o igual a 0.")]

        public decimal Precio { get; set; }
        public string? Imagen { get; set; } = "https://via.placeholder.com/150";
        public int? ServicioId { get; set; }

        // // Relación con Clientes
        // public ICollection<ClienteDto> Clientes { get; set; }
        //
        // // Relación con Vehiculos
        // public ICollection<VehiculoDto> Vehiculos { get; set; }

        // Auditoría
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
        
    }
}