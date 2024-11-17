using TallerMecanico.models;

namespace TallerMecanico.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public CategoriaProducto Categoria { get; set; }
        public string Imagen { get; set; }
        // Relación con Carrito
      //  public ICollection<CarritoDto>? Carritos { get; set; }

        // Auditoria
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public bool EstaBorrado { get; set; } = false;
    }
}