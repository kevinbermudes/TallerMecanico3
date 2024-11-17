using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        // Obtener todos los vehículos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetAllVehiculos()
        {
            var vehiculos = await _vehiculoService.GetAllVehiculosAsync();
            return Ok(vehiculos);
        }

        // Obtener un vehículo específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoDto>> GetVehiculoById(int id)
        {
            try
            {
                var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id);
                return Ok(vehiculo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo vehículo
        [HttpPost]
        public async Task<ActionResult<VehiculoDto>> CreateVehiculo(VehiculoDto vehiculoDto)
        {
            var nuevoVehiculo = await _vehiculoService.CreateVehiculoAsync(vehiculoDto);
            return CreatedAtAction(nameof(GetVehiculoById), new { id = nuevoVehiculo.Id }, nuevoVehiculo);
        }

        // Actualizar un vehículo existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehiculo(int id, VehiculoDto vehiculoDto)
        {
            try
            {
                await _vehiculoService.UpdateVehiculoAsync(id, vehiculoDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de un vehículo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            try
            {
                await _vehiculoService.DeleteVehiculoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener vehículos por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetVehiculosByClienteId(int clienteId)
        {
            var vehiculos = await _vehiculoService.GetVehiculosByClienteIdAsync(clienteId);
            return Ok(vehiculos);
        }

        // Agregar servicio a un vehículo
        [HttpPost("{vehiculoId}/servicio/{servicioId}")]
        public async Task<IActionResult> AddServicioToVehiculo(int vehiculoId, int servicioId)
        {
            try
            {
                await _vehiculoService.AddServicioToVehiculoAsync(vehiculoId, servicioId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Eliminar servicio de un vehículo
        [HttpDelete("{vehiculoId}/servicio/{servicioId}")]
        public async Task<IActionResult> RemoveServicioFromVehiculo(int vehiculoId, int servicioId)
        {
            try
            {
                await _vehiculoService.RemoveServicioFromVehiculoAsync(vehiculoId, servicioId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
