using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // Obtener todos los clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAllClientes()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes);
        }

        // Obtener un cliente específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetClienteById(int id)
        {
            try
            {
                var cliente = await _clienteService.GetClienteByIdAsync(id);
                return Ok(cliente);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo cliente
        [HttpPost]
        public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] ClienteDto clienteDto)
        {
            var nuevoCliente = await _clienteService.CreateClienteAsync(clienteDto);
            return CreatedAtAction(nameof(GetClienteById), new { id = nuevoCliente.Id }, nuevoCliente);
        }

        // Actualizar un cliente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, ClienteDto clienteDto)
        {
            try
            {
                await _clienteService.UpdateClienteAsync(id, clienteDto);

                // Devuelve una respuesta indicando éxito
                return Ok(new { success = true, message = "Cliente actualizado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error al actualizar el cliente." });
            }
        }


        // Borrado lógico de un cliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                await _clienteService.DeleteClienteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener cartas de pago por ClienteId
        [HttpGet("{id}/cartas-pago")]
        public async Task<ActionResult<IEnumerable<CartaPagoDto>>> GetCartasPagoByClienteId(int id)
        {
            var cartasPago = await _clienteService.GetCartasPagoByClienteIdAsync(id);
            return Ok(cartasPago);
        }

        // Obtener facturas por ClienteId
        [HttpGet("{id}/facturas")]
        public async Task<ActionResult<IEnumerable<FacturaDto>>> GetFacturasByClienteId(int id)
        {
            var facturas = await _clienteService.GetFacturasByClienteIdAsync(id);
            return Ok(facturas);
        }

        // Obtener notificaciones por ClienteId
        [HttpGet("{id}/notificaciones")]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetNotificacionesByClienteId(int id)
        {
            var notificaciones = await _clienteService.GetNotificacionesByClienteIdAsync(id);
            return Ok(notificaciones);
        }

        // Obtener pagos por ClienteId
        [HttpGet("{id}/pagos")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByClienteId(int id)
        {
            var pagos = await _clienteService.GetPagosByClienteIdAsync(id);
            return Ok(pagos);
        }

        // Obtener servicios por ClienteId
        [HttpGet("{id}/servicios")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServiciosByClienteId(int id)
        {
            var servicios = await _clienteService.GetServiciosByClienteIdAsync(id);
            return Ok(servicios);
        }

        // Obtener vehículos por ClienteId
        [HttpGet("{id}/vehiculos")]
        public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetVehiculosByClienteId(int id)
        {
            var vehiculos = await _clienteService.GetVehiculosByClienteIdAsync(id);
            return Ok(vehiculos);
        }
    }
}
