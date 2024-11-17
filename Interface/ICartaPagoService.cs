using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface ICartaPagoService
    {
        // Obtener todas las cartas de pago
        Task<IEnumerable<CartaPagoDto>> GetAllCartasPagoAsync();

        // Obtener una carta de pago específica por ID, incluyendo Cliente y Factura
        Task<CartaPagoDto> GetCartaPagoByIdAsync(int id);

        // Crear una nueva carta de pago
        Task<CartaPagoDto> CreateCartaPagoAsync(CartaPagoDto cartaPagoDto);

        // Actualizar una carta de pago existente
        Task UpdateCartaPagoAsync(int id, CartaPagoDto cartaPagoDto);

        // Borrado lógico de una carta de pago
        Task DeleteCartaPagoAsync(int id);

        // Métodos específicos (si aplica)
        Task<IEnumerable<CartaPagoDto>> GetCartasPagoByClienteIdAsync(int clienteId);
        Task<IEnumerable<CartaPagoDto>> GetCartasPagoByFacturaIdAsync(int facturaId);
    }
}