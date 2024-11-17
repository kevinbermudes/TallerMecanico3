using TallerMecanico.Dtos;

namespace TallerMecanico.Interface
{
    public interface IParteService
    {
        // Obtener todos los partes
        Task<IEnumerable<ParteDto>> GetAllPartesAsync();

        // Obtener un parte específico por ID, incluyendo Cliente
        Task<ParteDto> GetParteByIdAsync(int id);

        // Crear un nuevo parte
        Task<ParteDto> CreateParteAsync(ParteDto parteDto);

        // Actualizar un parte existente
        Task UpdateParteAsync(int id, ParteDto parteDto);

        // Borrado lógico de un parte
        Task DeleteParteAsync(int id);

        // Métodos específicos
        Task<IEnumerable<ParteDto>> GetPartesByClienteIdAsync(int clienteId);
    }
}