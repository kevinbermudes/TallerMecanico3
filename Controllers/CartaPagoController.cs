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

        // Crear una nueva carta de pago
        [HttpPost]
        public async Task<ActionResult<CartaPagoDto>> CreateCartaPago(CartaPagoDto cartaPagoDto)
        {
            var nuevaCartaPago = await _cartaPagoService.CreateCartaPagoAsync(cartaPagoDto);
            return CreatedAtAction(nameof(GetCartaPagoById), new { id = nuevaCartaPago.Id }, nuevaCartaPago);
        }

        // Actualizar una carta de pago existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartaPago(int id, CartaPagoDto cartaPagoDto)
        {
            try
            {
                await _cartaPagoService.UpdateCartaPagoAsync(id, cartaPagoDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de una carta de pago
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartaPago(int id)
        {
            try
            {
                await _cartaPagoService.DeleteCartaPagoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener cartas de pago por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<CartaPagoDto>>> GetCartasPagoByClienteId(int clienteId)
        {
            var cartasPago = await _cartaPagoService.GetCartasPagoByClienteIdAsync(clienteId);
            return Ok(cartasPago);
        }

        // Obtener cartas de pago por factura
        [HttpGet("factura/{facturaId}")]
        public async Task<ActionResult<IEnumerable<CartaPagoDto>>> GetCartasPagoByFacturaId(int facturaId)
        {
            var cartasPago = await _cartaPagoService.GetCartasPagoByFacturaIdAsync(facturaId);
            return Ok(cartasPago);
        }
    }
}
