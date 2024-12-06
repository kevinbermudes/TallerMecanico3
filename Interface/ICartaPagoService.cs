using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface ICartaPagoService
    {
        // Obtener todas las cartas de pago
        Task<IEnumerable<CartaPagoDto>> GetAllCartasPagoAsync();

        // Obtener una carta de pago específica por ID, incluyendo Cliente y Facturas asociadas
        Task<CartaPagoDto> GetCartaPagoByIdAsync(int id);

        // Crear o actualizar una carta de pago según las facturas pendientes
        Task<CartaPagoDto> CreateOrUpdateCartaPagoAsync(int clienteId, DateTime fechaVencimiento);

        // Eliminar lógicamente una carta de pago si no tiene facturas pendientes
        Task DeleteCartaPagoIfNoPendingInvoicesAsync(int cartaPagoId);

        // Obtener todas las cartas de pago por cliente, incluyendo las facturas asociadas
        Task<IEnumerable<CartaPagoDto>> GetCartasPagoByClienteIdAsync(int clienteId);
        Task<IEnumerable<FacturaDto>> GetFacturasByCartaPagoIdAsync(int cartaPagoId);

    }
}