using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public NotificacionService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todas las notificaciones
        public async Task<IEnumerable<NotificacionDto>> GetAllNotificacionesAsync()
        {
            var notificaciones = await _context.Notificaciones
                .Include(n => n.Cliente)
                .Where(n => !n.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NotificacionDto>>(notificaciones);
        }

        // Obtener una notificación específica por ID
        public async Task<NotificacionDto> GetNotificacionByIdAsync(int id)
        {
            var notificacion = await _context.Notificaciones
                .Include(n => n.Cliente)
                .FirstOrDefaultAsync(n => n.Id == id && !n.EstaBorrado);

            if (notificacion == null)
                throw new KeyNotFoundException("Notificación no encontrada.");

            return _mapper.Map<NotificacionDto>(notificacion);
        }

        // Crear una nueva notificación
        public async Task<NotificacionDto> CreateNotificacionAsync(NotificacionDto notificacionDto)
        {
            var notificacion = _mapper.Map<Notificacion>(notificacionDto);
            notificacion.FechaCreacion = DateTime.UtcNow;
            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();

            return _mapper.Map<NotificacionDto>(notificacion);
        }

        // Actualizar una notificación existente
        public async Task UpdateNotificacionAsync(int id, NotificacionDto notificacionDto)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);

            if (notificacion == null || notificacion.EstaBorrado)
                throw new KeyNotFoundException("Notificación no encontrada o ha sido eliminada.");

            _mapper.Map(notificacionDto, notificacion);
            notificacion.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de una notificación
        public async Task DeleteNotificacionAsync(int id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);

            if (notificacion == null || notificacion.EstaBorrado)
                throw new KeyNotFoundException("Notificación no encontrada o ya ha sido eliminada.");

            notificacion.EstaBorrado = true;
            notificacion.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener notificaciones por cliente
        public async Task<IEnumerable<NotificacionDto>> GetNotificacionesByClienteIdAsync(int clienteId)
        {
            var notificaciones = await _context.Notificaciones
                .Where(n => n.ClienteId == clienteId && !n.EstaBorrado)
                .Include(n => n.Cliente)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NotificacionDto>>(notificaciones);
        }

        // Obtener notificaciones por tipo
        public async Task<IEnumerable<NotificacionDto>> GetNotificacionesByTipoAsync(TipoNotificacion tipo)
        {
            var notificaciones = await _context.Notificaciones
                .Where(n => n.Tipo == tipo && !n.EstaBorrado)
                .Include(n => n.Cliente)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NotificacionDto>>(notificaciones);
        }

        // Marcar notificación como leída
        public async Task MarcarComoLeidoAsync(int id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);

            if (notificacion == null || notificacion.EstaBorrado)
                throw new KeyNotFoundException("Notificación no encontrada o ya ha sido eliminada.");

            notificacion.Leido = true;
            await _context.SaveChangesAsync();
        }
    }
}
