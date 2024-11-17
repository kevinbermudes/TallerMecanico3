using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]  
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAllUsuarios()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }

        // Obtener un usuario específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                return Ok(usuario);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo usuario (registro)
        [HttpPost]
        [AllowAnonymous]  
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromBody] UsuarioDto usuarioDto, [FromQuery] string password)
        {
            var nuevoUsuario = await _usuarioService.CreateUsuarioAsync(usuarioDto, password);
            return CreatedAtAction(nameof(GetUsuarioById), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        // Actualizar un usuario existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UsuarioDto usuarioDto)
        {
            try
            {
                await _usuarioService.UpdateUsuarioAsync(id, usuarioDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Borrado lógico de un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                await _usuarioService.DeleteUsuarioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Autenticación del usuario
        [HttpPost("authenticate")]
        [AllowAnonymous]  // Permitir acceso anónimo para autenticación
        public async Task<ActionResult<UsuarioDto>> Authenticate([FromBody] string email, string password)
        {
            try
            {
                var usuario = await _usuarioService.AuthenticateAsync(email, password);
                return Ok(usuario);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // Cambiar contraseña del usuario
        [HttpPatch("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] string newPassword)
        {
            try
            {
                await _usuarioService.ChangePasswordAsync(id, newPassword);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Alternar el estado del usuario
        [HttpPatch("{id}/toggle-estado")]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            try
            {
                await _usuarioService.ToggleEstadoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener usuarios por rol
        [HttpGet("rol/{rol}")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuariosByRol(RolUsuario rol)
        {
            var usuarios = await _usuarioService.GetUsuariosByRolAsync(rol);
            return Ok(usuarios);
        }
    }
}
