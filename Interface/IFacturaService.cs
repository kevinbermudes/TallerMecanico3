using TallerMecanico.Dtos;
using TallerMecanico.models;

namespace TallerMecanico.Interface
{
    public interface IFacturaService
    {
        // Obtener todas las facturas
        Task<IEnumerable<FacturaDto>> GetAllFacturasAsync();

        // Obtener una factura específica por ID, incluyendo Cliente y CartasPago
        Task<FacturaDto> GetFacturaByIdAsync(int id);

        // Crear una nueva factura
        Task<FacturaDto> CreateFacturaAsync(FacturaDto facturaDto);

        // Actualizar una factura existente
        Task UpdateFacturaAsync(int id, FacturaDto facturaDto);

        // Borrado lógico de una factura
        Task DeleteFacturaAsync(int id);

        // Métodos específicos (si aplica)
        Task<IEnumerable<FacturaDto>> GetFacturasByClienteIdAsync(int clienteId);
        Task<IEnumerable<FacturaDto>> GetFacturasByEstadoAsync(EstadoFactura estado);
    }
}