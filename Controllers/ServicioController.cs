using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServicioController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        // Obtener todos los servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetAllServicios()
        {
            var servicios = await _servicioService.GetAllServiciosAsync();
            return Ok(servicios);
        }

        // Obtener un servicio específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetServicioById(int id)
        {
            try
            {
                var servicio = await _servicioService.GetServicioByIdAsync(id);
                return Ok(servicio);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo servicio
        [HttpPost]
        public async Task<ActionResult<ServicioDto>> CreateServicio(ServicioDto servicioDto)
        {
            var nuevoServicio = await _servicioService.CreateServicioAsync(servicioDto);
            return CreatedAtAction(nameof(GetServicioById), new { id = nuevoServicio.Id }, nuevoServicio);
        }

        // Actualizar un servicio existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, ServicioDto servicioDto)
        {
            try
            {
                await _servicioService.UpdateServicioAsync(id, servicioDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de un servicio
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                await _servicioService.DeleteServicioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener servicios por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServiciosByClienteId(int clienteId)
        {
            var servicios = await _servicioService.GetServiciosByClienteIdAsync(clienteId);
            return Ok(servicios);
        }

        // Agregar cliente a un servicio
        [HttpPost("{servicioId}/cliente/{clienteId}")]
        public async Task<IActionResult> AddClienteToServicio(int servicioId, int clienteId)
        {
            try
            {
                await _servicioService.AddClienteToServicioAsync(servicioId, clienteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Eliminar cliente de un servicio
        [HttpDelete("{servicioId}/cliente/{clienteId}")]
        public async Task<IActionResult> RemoveClienteFromServicio(int servicioId, int clienteId)
        {
            try
            {
                await _servicioService.RemoveClienteFromServicioAsync(servicioId, clienteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
