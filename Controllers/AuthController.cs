using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
        private readonly IClienteService _clienteService;

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _usuarioService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                var token = GenerateJwtToken(user);
        
                return Ok(new { token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized("Credenciales inválidas.");
            }
        }



        private string GenerateJwtToken(UsuarioDto user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
             //   new Claim(ClaimTypes.Name, user.Email),
                new Claim("email", user.Email),
                new Claim("nombre", user.Nombre),
                new Claim("role", user.Rol.ToString())
                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDto registroDto)
        {
            try
            {
                await _usuarioService.RegistrarUsuarioYClienteAsync(registroDto);
                return Ok(new { message = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("El correo electrónico ya está en uso."))
                {
                    return Conflict(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Error al registrar usuario y cliente.", details = ex.Message });
            }
        }


    

    }
}