using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        // Obtener todas las notificaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetAllNotificaciones()
        {
            var notificaciones = await _notificacionService.GetAllNotificacionesAsync();
            return Ok(notificaciones);
        }

        // Obtener una notificación específica por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificacionDto>> GetNotificacionById(int id)
        {
            try
            {
                var notificacion = await _notificacionService.GetNotificacionByIdAsync(id);
                return Ok(notificacion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear una nueva notificación
        [HttpPost]
        public async Task<ActionResult<NotificacionDto>> CreateNotificacion(NotificacionDto notificacionDto)
        {
            var nuevaNotificacion = await _notificacionService.CreateNotificacionAsync(notificacionDto);
            return CreatedAtAction(nameof(GetNotificacionById), new { id = nuevaNotificacion.Id }, nuevaNotificacion);
        }

        // Actualizar una notificación existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotificacion(int id, NotificacionDto notificacionDto)
        {
            try
            {
                await _notificacionService.UpdateNotificacionAsync(id, notificacionDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de una notificación
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificacion(int id)
        {
            try
            {
                await _notificacionService.DeleteNotificacionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener notificaciones por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetNotificacionesByClienteId(int clienteId)
        {
            var notificaciones = await _notificacionService.GetNotificacionesByClienteIdAsync(clienteId);
            return Ok(notificaciones);
        }

        // Obtener notificaciones por tipo
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetNotificacionesByTipo(TipoNotificacion tipo)
        {
            var notificaciones = await _notificacionService.GetNotificacionesByTipoAsync(tipo);
            return Ok(notificaciones);
        }

        // Marcar notificación como leída
        [HttpPatch("{id}/marcar-como-leido")]
        public async Task<IActionResult> MarcarComoLeido(int id)
        {
            try
            {
                await _notificacionService.MarcarComoLeidoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
