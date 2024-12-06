using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaPagoController : ControllerBase
    {
        private readonly ICartaPagoService _cartaPagoService;

        public CartaPagoController(ICartaPagoService cartaPagoService)
        {
            _cartaPagoService = cartaPagoService;
        }

        // Obtener todas las cartas de pago
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartaPagoDto>>> GetAllCartasPago()
        {
            var cartasPago = await _cartaPagoService.GetAllCartasPagoAsync();
            return Ok(cartasPago);
        }

        // Obtener una carta de pago específica por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CartaPagoDto>> GetCartaPagoById(int id)
        {
            try
            {
                var cartaPago = await _cartaPagoService.GetCartaPagoByIdAsync(id);
                return Ok(cartaPago);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear o actualizar una carta de pago
        [HttpPost("create-or-update")]
        public async Task<ActionResult<CartaPagoDto>> CreateOrUpdateCartaPago(int clienteId, DateTime fechaVencimiento)
        {
            try
            {
                var cartaPago = await _cartaPagoService.CreateOrUpdateCartaPagoAsync(clienteId, fechaVencimiento);
                return Ok(cartaPago);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al crear o actualizar la carta de pago.", details = ex.Message });
            }
        }

        // Eliminar una carta de pago lógicamente si no tiene facturas pendientes
        [HttpDelete("{id}/check-delete")]
        public async Task<IActionResult> DeleteCartaPagoIfNoPendingInvoices(int id)
        {
            try
            {
                await _cartaPagoService.DeleteCartaPagoIfNoPendingInvoicesAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar la carta de pago.", details = ex.Message });
            }
        }

        // Obtener cartas de pago por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<CartaPagoDto>>> GetCartasPagoByClienteId(int clienteId)
        {
            var cartasPago = await _cartaPagoService.GetCartasPagoByClienteIdAsync(clienteId);

            if (cartasPago == null || !cartasPago.Any())
            {
                return Ok(new List<CartaPagoDto>()); // Devuelve una lista vacía si no hay datos
            }

            return Ok(cartasPago);
        }

        // Obtener facturas asociadas a una carta de pago
        [HttpGet("{id}/facturas")]
        public async Task<ActionResult<IEnumerable<FacturaDto>>> GetFacturasByCartaPagoId(int id)
        {
            try
            {
                var facturas = await _cartaPagoService.GetFacturasByCartaPagoIdAsync(id);
                return Ok(facturas);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al obtener las facturas de la carta de pago.", details = ex.Message });
            }
        }
    }
}
