namespace TallerMecanico.models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Imagen { get; set; }
    public CategoriaProducto Categoria { get; set; } 
    public ICollection<Carrito> Carritos { get; set; } 


    // Auditoría
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaBorrado { get; set; }
    public bool EstaBorrado { get; set; } = false;
}

public enum CategoriaProducto
{
    Herramientas,
    Repuestos,
    Lubricantes,
    Accesorios,
    Otros
}
