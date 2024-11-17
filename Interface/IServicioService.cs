using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface IServicioService
    {
        // Obtener todos los servicios
        Task<IEnumerable<ServicioDto>> GetAllServiciosAsync();

        // Obtener un servicio específico por ID, incluyendo los clientes y vehículos asociados
        Task<ServicioDto> GetServicioByIdAsync(int id);

        // Crear un nuevo servicio
        Task<ServicioDto> CreateServicioAsync(ServicioDto servicioDto);

        // Actualizar un servicio existente
        Task UpdateServicioAsync(int id, ServicioDto servicioDto);

        // Borrado lógico de un servicio
        Task DeleteServicioAsync(int id);

        // Métodos específicos
        Task<IEnumerable<ServicioDto>> GetServiciosByClienteIdAsync(int clienteId);
        Task<IEnumerable<ServicioDto>> GetServiciosByVehiculoIdAsync(int vehiculoId); // Nueva función

        // Métodos para manejar la relación muchos a muchos con Cliente
        Task AddClienteToServicioAsync(int servicioId, int clienteId);
        Task RemoveClienteFromServicioAsync(int servicioId, int clienteId);

        // Métodos para manejar la relación muchos a muchos con Vehiculo
        Task AddVehiculoToServicioAsync(int servicioId, int vehiculoId); // Nueva función
        Task RemoveVehiculoFromServicioAsync(int servicioId, int vehiculoId); // Nueva función
    }
}