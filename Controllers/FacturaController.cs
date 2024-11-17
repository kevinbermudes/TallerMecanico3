using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        // Obtener todas las facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaDto>>> GetAllFacturas()
        {
            var facturas = await _facturaService.GetAllFacturasAsync();
            return Ok(facturas);
        }

        // Obtener una factura específica por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaDto>> GetFacturaById(int id)
        {
            try
            {
                var factura = await _facturaService.GetFacturaByIdAsync(id);
                return Ok(factura);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear una nueva factura
        [HttpPost]
        public async Task<ActionResult<FacturaDto>> CreateFactura(FacturaDto facturaDto)
        {
            var nuevaFactura = await _facturaService.CreateFacturaAsync(facturaDto);
            return CreatedAtAction(nameof(GetFacturaById), new { id = nuevaFactura.Id }, nuevaFactura);
        }

        // Actualizar una factura existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFactura(int id, FacturaDto facturaDto)
        {
            try
            {
                await _facturaService.UpdateFacturaAsync(id, facturaDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de una factura
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            try
            {
                await _facturaService.DeleteFacturaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener facturas por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<FacturaDto>>> GetFacturasByClienteId(int clienteId)
        {
            var facturas = await _facturaService.GetFacturasByClienteIdAsync(clienteId);
            return Ok(facturas);
        }

        // Obtener facturas por estado
        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<FacturaDto>>> GetFacturasByEstado(EstadoFactura estado)
        {
            var facturas = await _facturaService.GetFacturasByEstadoAsync(estado);
            return Ok(facturas);
        }
    }
}
