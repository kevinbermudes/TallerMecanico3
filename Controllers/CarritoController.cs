using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        // Obtener todos los carritos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarritoDto>>> GetAllCarritos()
        {
            var carritos = await _carritoService.GetAllCarritosAsync();
            return Ok(carritos);
        }

        // Obtener un carrito específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CarritoDto>> GetCarritoById(int id)
        {
            try
            {
                var carrito = await _carritoService.GetCarritoByIdAsync(id);
                return Ok(carrito);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo carrito
        [HttpPost]
        public async Task<ActionResult<CarritoDto>> CreateCarrito(CarritoDto carritoDto)
        {
            var nuevoCarrito = await _carritoService.CreateCarritoAsync(carritoDto);
            return CreatedAtAction(nameof(GetCarritoById), new { id = nuevoCarrito.Id }, nuevoCarrito);
        }

        // Actualizar un carrito existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarrito(int id, CarritoDto carritoDto)
        {
            try
            {
                await _carritoService.UpdateCarritoAsync(id, carritoDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de un carrito
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrito(int id)
        {
            try
            {
                await _carritoService.DeleteCarritoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener carritos por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<CarritoDto>>> GetCarritosByClienteId(int clienteId)
        {
            var carritos = await _carritoService.GetCarritosByClienteIdAsync(clienteId);
            return Ok(carritos);
        }

        // Obtener carritos por producto
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<CarritoDto>>> GetCarritosByProductoId(int productoId)
        {
            var carritos = await _carritoService.GetCarritosByProductoIdAsync(productoId);
            return Ok(carritos);
        }
        [HttpPost("agregar-al-carrito")]
        public async Task<IActionResult> AgregarAlCarrito([FromBody] CarritoAgregarDto carritoAgregarDto)
        {
            try
            {
                var carritoCreado = await _carritoService.AgregarAlCarritoAsync(carritoAgregarDto);
                return CreatedAtAction(nameof(GetCarritoById), new { id = carritoCreado.Id }, carritoCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al agregar el producto al carrito.", details = ex.Message });
            }
        }

    }
}
