using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface ICarritoService
    {
        // Obtener todos los carritos
        Task<IEnumerable<CarritoDto>> GetAllCarritosAsync();

        // Obtener un carrito específico por ID, incluyendo Cliente y Producto
        Task<CarritoDto> GetCarritoByIdAsync(int id);

        // Crear un nuevo carrito
        Task<CarritoDto> CreateCarritoAsync(CarritoDto carritoDto);

        // Actualizar un carrito existente
        Task UpdateCarritoAsync(int id, CarritoDto carritoDto);

        // Borrado lógico de un carrito
        Task DeleteCarritoAsync(int id);

        // Metodos específicos (si aplica)
        Task<IEnumerable<CarritoDto>> GetCarritosByClienteIdAsync(int clienteId);
        Task<IEnumerable<CarritoDto>> GetCarritosByProductoIdAsync(int productoId);
        Task<CarritoDto> AgregarAlCarritoAsync(CarritoAgregarDto carritoAgregarDto);
    }
}