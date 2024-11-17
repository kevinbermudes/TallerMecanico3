using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;
using System.Security.Cryptography;
using System.Text;

namespace TallerMecanico.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuarios = await _context.Usuarios
                .Where(u => !u.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        // Obtener un usuario específico por ID
        public async Task<UsuarioDto> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && !u.EstaBorrado);

            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            return _mapper.Map<UsuarioDto>(usuario);
        }

        // Crear un nuevo usuario (registro)
        public async Task<UsuarioDto> CreateUsuarioAsync(UsuarioDto usuarioDto, string password)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.PasswordHash = HashPassword(password);
            usuario.FechaCreacion = DateTime.UtcNow;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        // Actualizar un usuario existente
        public async Task UpdateUsuarioAsync(int id, UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.EstaBorrado)
                throw new KeyNotFoundException("Usuario no encontrado o ha sido eliminado.");

            _mapper.Map(usuarioDto, usuario);
            usuario.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de un usuario
        public async Task DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.EstaBorrado)
                throw new KeyNotFoundException("Usuario no encontrado o ya ha sido eliminado.");

            usuario.EstaBorrado = true;
            usuario.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Autenticación del usuario
        public async Task<UsuarioDto> AuthenticateAsync(string email, string password)
        {
            // Verificar si el usuario existe y no está marcado como borrado
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && !u.EstaBorrado);

            if (usuario == null)
            {
                Console.WriteLine($"Usuario no encontrado o marcado como borrado. Email: {email}");
                throw new UnauthorizedAccessException("Credenciales inválidas o el usuario ha sido eliminado.");
            }

            // Verificar contraseña
            if (!VerifyPassword(password, usuario.PasswordHash))
            {
                Console.WriteLine("Contraseña incorrecta para el usuario con email: " + email);
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            Console.WriteLine("Usuario autenticado correctamente. ID: " + usuario.Id);
            return _mapper.Map<UsuarioDto>(usuario);
        }



        // Cambiar la contraseña del usuario
        public async Task ChangePasswordAsync(int id, string newPassword)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.EstaBorrado)
                throw new KeyNotFoundException("Usuario no encontrado o ha sido eliminado.");

            usuario.PasswordHash = HashPassword(newPassword);
            usuario.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Alternar el estado del usuario (activar/desactivar)
        public async Task ToggleEstadoAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.EstaBorrado)
                throw new KeyNotFoundException("Usuario no encontrado o ha sido eliminado.");

            usuario.Estado = !usuario.Estado;
            usuario.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener usuarios por rol
        public async Task<IEnumerable<UsuarioDto>> GetUsuariosByRolAsync(RolUsuario rol)
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Rol == rol && !u.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }
        
        public async Task RegistrarUsuarioYClienteAsync(RegistroDto registroDto)
        {
            try
            {
                // Crear y mapear el Usuario
                var usuario = new Usuario
                {
                    Nombre = registroDto.Nombre,
                    Apellido = registroDto.Apellido,
                    Email = registroDto.Email,
                    PasswordHash = HashPassword(registroDto.Password),
                    Rol = RolUsuario.Cliente,
                    FechaCreacion = DateTime.UtcNow,
                    FechaRegistro = DateTime.UtcNow 
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Crear y mapear el Cliente asociado al Usuario
                var cliente = new Cliente
                {
                    UsuarioId = usuario.Id,
                    Direccion = registroDto.Direccion,
                    Telefono = registroDto.Telefono,
                    FechaNacimiento = registroDto.FechaNacimiento?.ToUniversalTime(),
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique") == true)
            {
                throw new Exception("El correo electrónico ya está en uso.");
            }
        }





        // Método para hash de contraseñas
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Método para verificar el hash de la contraseña
        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
