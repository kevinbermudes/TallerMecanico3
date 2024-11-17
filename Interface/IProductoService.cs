using TallerMecanico.Dtos;
using TallerMecanico.models;

namespace TallerMecanico.Interface
{
    public interface IProductoService
    {
        // Obtener todos los productos
        Task<IEnumerable<ProductoDto>> GetAllProductosAsync();

        // Obtener un producto específico por ID, incluyendo los carritos asociados
        Task<ProductoDto> GetProductoByIdAsync(int id);

        // Crear un nuevo producto
        Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto);

        // Actualizar un producto existente
        Task UpdateProductoAsync(int id, ProductoDto productoDto);

        // Borrado lógico de un producto
        Task DeleteProductoAsync(int id);

        // Métodos específicos
        Task<IEnumerable<ProductoDto>> GetProductosByCategoriaAsync(CategoriaProducto categoria);
        Task UpdateStockAsync(int id, int cantidad);
    }
}