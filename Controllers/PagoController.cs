using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        // Obtener todos los pagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetAllPagos()
        {
            var pagos = await _pagoService.GetAllPagosAsync();
            return Ok(pagos);
        }

        // Obtener un pago específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDto>> GetPagoById(int id)
        {
            try
            {
                var pago = await _pagoService.GetPagoByIdAsync(id);
                return Ok(pago);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo pago
        [HttpPost]
        public async Task<ActionResult<PagoDto>> CreatePago(PagoDto pagoDto)
        {
            var nuevoPago = await _pagoService.CreatePagoAsync(pagoDto);
            return CreatedAtAction(nameof(GetPagoById), new { id = nuevoPago.Id }, nuevoPago);
        }

        // Actualizar un pago existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, PagoDto pagoDto)
        {
            try
            {
                await _pagoService.UpdatePagoAsync(id, pagoDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de un pago
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            try
            {
                await _pagoService.DeletePagoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener pagos por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByClienteId(int clienteId)
        {
            var pagos = await _pagoService.GetPagosByClienteIdAsync(clienteId);
            return Ok(pagos);
        }

        // Obtener pagos por tipo de pago
        [HttpGet("tipo/{tipoPago}")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByTipoPago(TipoPago tipoPago)
        {
            var pagos = await _pagoService.GetPagosByTipoPagoAsync(tipoPago);
            return Ok(pagos);
        }

        // Obtener pagos por producto
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByProductoId(int productoId)
        {
            var pagos = await _pagoService.GetPagosByProductoIdAsync(productoId);
            return Ok(pagos);
        }

        // Obtener pagos por servicio
        [HttpGet("servicio/{servicioId}")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByServicioId(int servicioId)
        {
            var pagos = await _pagoService.GetPagosByServicioIdAsync(servicioId);
            return Ok(pagos);
        }

        // Obtener pagos por parte
        [HttpGet("parte/{parteId}")]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByParteId(int parteId)
        {
            var pagos = await _pagoService.GetPagosByParteIdAsync(parteId);
            return Ok(pagos);
        }
    }
}
