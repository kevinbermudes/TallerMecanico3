using TallerMecanico.Dtos;
using TallerMecanico.models;

namespace TallerMecanico.Interface
{
    public interface IPagoService
    {
        // Obtener todos los pagos
        Task<IEnumerable<PagoDto>> GetAllPagosAsync();

        // Obtener un pago específico por ID, incluyendo Cliente y el objeto pagado (Producto, Servicio, Parte)
        Task<PagoDto> GetPagoByIdAsync(int id);

        // Crear un nuevo pago
        Task<PagoDto> CreatePagoAsync(PagoDto pagoDto);

        // Actualizar un pago existente
        Task UpdatePagoAsync(int id, PagoDto pagoDto);

        // Borrado lógico de un pago
        Task DeletePagoAsync(int id);

        // Métodos específicos
        Task<IEnumerable<PagoDto>> GetPagosByClienteIdAsync(int clienteId);
        Task<IEnumerable<PagoDto>> GetPagosByTipoPagoAsync(TipoPago tipoPago);
        Task<IEnumerable<PagoDto>> GetPagosByProductoIdAsync(int productoId);
        Task<IEnumerable<PagoDto>> GetPagosByServicioIdAsync(int servicioId);
        Task<IEnumerable<PagoDto>> GetPagosByParteIdAsync(int parteId);
    }
}