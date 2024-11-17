using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface IVehiculoService
    {
        // Obtener todos los vehículos
        Task<IEnumerable<VehiculoDto>> GetAllVehiculosAsync();

        // Obtener un vehículo específico por ID, incluyendo Cliente y ServiciosRealizados
        Task<VehiculoDto> GetVehiculoByIdAsync(int id);

        // Crear un nuevo vehículo
        Task<VehiculoDto> CreateVehiculoAsync(VehiculoDto vehiculoDto);

        // Actualizar un vehículo existente
        Task UpdateVehiculoAsync(int id, VehiculoDto vehiculoDto);

        // Borrado lógico de un vehículo
        Task DeleteVehiculoAsync(int id);

        // Métodos específicos
        Task<IEnumerable<VehiculoDto>> GetVehiculosByClienteIdAsync(int clienteId);
        Task AddServicioToVehiculoAsync(int vehiculoId, int servicioId);
        Task RemoveServicioFromVehiculoAsync(int vehiculoId, int servicioId);
    }
}