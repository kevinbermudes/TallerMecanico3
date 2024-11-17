using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface IClienteService
    {
        // Obtener todos los clientes
        Task<IEnumerable<ClienteDto>> GetAllClientesAsync();

        // Obtener un cliente específico por ID
        Task<ClienteDto> GetClienteByIdAsync(int id);

        // Crear un nuevo cliente
        Task<ClienteDto> CreateClienteAsync(ClienteDto clienteDto);

        // Actualizar un cliente existente
        Task UpdateClienteAsync(int id, ClienteDto clienteDto);

        // Borrado lógico de un cliente
        Task DeleteClienteAsync(int id);

        // Obtener clientes por UsuarioID
        Task<ClienteDto> GetClienteByUsuarioIdAsync(int usuarioId);

        // Obtener vehículos de un cliente específico
        Task<IEnumerable<VehiculoDto>> GetVehiculosByClienteIdAsync(int clienteId);

        // Obtener servicios de un cliente específico
        Task<IEnumerable<ServicioDto>> GetServiciosByClienteIdAsync(int clienteId);

        // Obtener facturas de un cliente específico
        Task<IEnumerable<FacturaDto>> GetFacturasByClienteIdAsync(int clienteId);

        // Obtener pagos de un cliente específico
        Task<IEnumerable<PagoDto>> GetPagosByClienteIdAsync(int clienteId);

        // Obtener cartas de pago de un cliente específico
        Task<IEnumerable<CartaPagoDto>> GetCartasPagoByClienteIdAsync(int clienteId);

        // Obtener notificaciones de un cliente específico
        Task<IEnumerable<NotificacionDto>> GetNotificacionesByClienteIdAsync(int clienteId);
    }
}