using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParteController : ControllerBase
    {
        private readonly IParteService _parteService;

        public ParteController(IParteService parteService)
        {
            _parteService = parteService;
        }

        // Obtener todos los partes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParteDto>>> GetAllPartes()
        {
            var partes = await _parteService.GetAllPartesAsync();
            return Ok(partes);
        }

        // Obtener un parte específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ParteDto>> GetParteById(int id)
        {
            try
            {
                var parte = await _parteService.GetParteByIdAsync(id);
                return Ok(parte);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo parte
        [HttpPost]
        public async Task<ActionResult<ParteDto>> CreateParte(ParteDto parteDto)
        {
            var nuevoParte = await _parteService.CreateParteAsync(parteDto);
            return CreatedAtAction(nameof(GetParteById), new { id = nuevoParte.Id }, nuevoParte);
        }

        // Actualizar un parte existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParte(int id, ParteDto parteDto)
        {
            try
            {
                await _parteService.UpdateParteAsync(id, parteDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de un parte
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParte(int id)
        {
            try
            {
                await _parteService.DeleteParteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener partes por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ParteDto>>> GetPartesByClienteId(int clienteId)
        {
            var partes = await _parteService.GetPartesByClienteIdAsync(clienteId);
            return Ok(partes);
        }
    }
}
