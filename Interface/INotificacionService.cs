using TallerMecanico.Dtos;
using TallerMecanico.models;

namespace TallerMecanico.Interface
{
    public interface INotificacionService
    {
        // Obtener todas las notificaciones
        Task<IEnumerable<NotificacionDto>> GetAllNotificacionesAsync();

        // Obtener una notificación específica por ID, incluyendo Cliente
        Task<NotificacionDto> GetNotificacionByIdAsync(int id);

        // Crear una nueva notificación
        Task<NotificacionDto> CreateNotificacionAsync(NotificacionDto notificacionDto);

        // Actualizar una notificación existente
        Task UpdateNotificacionAsync(int id, NotificacionDto notificacionDto);

        // Borrado lógico de una notificación
        Task DeleteNotificacionAsync(int id);

        // Métodos específicos
        Task<IEnumerable<NotificacionDto>> GetNotificacionesByClienteIdAsync(int clienteId);
        Task<IEnumerable<NotificacionDto>> GetNotificacionesByTipoAsync(TipoNotificacion tipo);
        Task MarcarComoLeidoAsync(int id);
    }
}