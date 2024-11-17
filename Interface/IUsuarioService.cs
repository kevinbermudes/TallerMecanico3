using TallerMecanico.Dtos;
using TallerMecanico.models;

namespace TallerMecanico.Interface
{
    public interface IUsuarioService
    {
        // Obtener todos los usuarios
        Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();

        // Obtener un usuario específico por ID
        Task<UsuarioDto> GetUsuarioByIdAsync(int id);

        // Crear un nuevo usuario (registro)
        Task<UsuarioDto> CreateUsuarioAsync(UsuarioDto usuarioDto, string password);

        // Actualizar un usuario existente
        Task UpdateUsuarioAsync(int id, UsuarioDto usuarioDto);

        // Borrado lógico de un usuario
        Task DeleteUsuarioAsync(int id);

        // Métodos específicos
        Task<UsuarioDto> AuthenticateAsync(string email, string password);
        Task ChangePasswordAsync(int id, string newPassword);
        Task ToggleEstadoAsync(int id);
        Task<IEnumerable<UsuarioDto>> GetUsuariosByRolAsync(RolUsuario rol);
        Task RegistrarUsuarioYClienteAsync(RegistroDto registroDto);

    }
}