using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class ClienteService : IClienteService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public ClienteService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los clientes
        public async Task<IEnumerable<ClienteDto>> GetAllClientesAsync()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Usuario)
                .Where(c => !c.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        // Obtener un cliente específico por ID
        public async Task<ClienteDto> GetClienteByIdAsync(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id && !c.EstaBorrado);

            if (cliente == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            return _mapper.Map<ClienteDto>(cliente);
        }

        // Crear un nuevo cliente
        public async Task<ClienteDto> CreateClienteAsync(ClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.FechaCreacion = DateTime.UtcNow;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return _mapper.Map<ClienteDto>(cliente);
        }

        // Actualizar un cliente existente
        public async Task UpdateClienteAsync(int id, ClienteDto clienteDto)
        {
            // Busca el cliente en la base de datos
            var cliente = await _context.Clientes.FindAsync(id);

            // Verifica si el cliente existe o ha sido eliminado
            if (cliente == null || cliente.EstaBorrado)
                throw new KeyNotFoundException("Cliente no encontrado o ha sido eliminado.");

            // Actualiza solo los campos del cliente (evita tocar relaciones)
            cliente.PrimerNombre = clienteDto.PrimerNombre;
            cliente.SegundoNombre = clienteDto.SegundoNombre;
            cliente.PrimerApellido = clienteDto.PrimerApellido;
            cliente.SegundoApellido = clienteDto.SegundoApellido;
            cliente.Direccion = clienteDto.Direccion;
            cliente.Telefono = clienteDto.Telefono;
            cliente.EmailSecundario = clienteDto.EmailSecundario;
            cliente.Dni = clienteDto.Dni;
            cliente.FechaNacimiento = clienteDto.FechaNacimiento;
            cliente.Genero = clienteDto.Genero;
            cliente.EstadoCivil = clienteDto.EstadoCivil;
            cliente.Ocupacion = clienteDto.Ocupacion;
            cliente.Notas = clienteDto.Notas;

            // Actualiza la fecha de modificación
            cliente.FechaActualizacion = DateTime.UtcNow;

            // Guarda los cambios
            await _context.SaveChangesAsync();
        }


        // Borrado lógico de un cliente
        public async Task DeleteClienteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null || cliente.EstaBorrado)
                throw new KeyNotFoundException("Cliente no encontrado o ya ha sido eliminado.");

            cliente.EstaBorrado = true;
            cliente.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener cliente por UsuarioId
        public async Task<ClienteDto> GetClienteByUsuarioIdAsync(int usuarioId)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && !c.EstaBorrado);

            if (cliente == null)
                throw new KeyNotFoundException("Cliente no encontrado para el usuario especificado.");

            return _mapper.Map<ClienteDto>(cliente);
        }

        // Obtener cartas de pago por ClienteId
        public async Task<IEnumerable<CartaPagoDto>> GetCartasPagoByClienteIdAsync(int clienteId)
        {
            var cartasPago = await _context.CartasPago
                .Where(cp => cp.ClienteId == clienteId && !cp.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
        }

        // Obtener facturas por ClienteId
        public async Task<IEnumerable<FacturaDto>> GetFacturasByClienteIdAsync(int clienteId)
        {
            var facturas = await _context.Facturas
                .Where(f => f.ClienteId == clienteId && !f.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FacturaDto>>(facturas);
        }

        // Obtener notificaciones por ClienteId
        public async Task<IEnumerable<NotificacionDto>> GetNotificacionesByClienteIdAsync(int clienteId)
        {
            var notificaciones = await _context.Notificaciones
                .Where(n => n.ClienteId == clienteId && !n.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NotificacionDto>>(notificaciones);
        }

        // Obtener pagos por ClienteId
        public async Task<IEnumerable<PagoDto>> GetPagosByClienteIdAsync(int clienteId)
        {
            var pagos = await _context.Pagos
                .Where(p => p.ClienteId == clienteId && !p.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }

        // Obtener servicios por ClienteId
        public async Task<IEnumerable<ServicioDto>> GetServiciosByClienteIdAsync(int clienteId)
        {
            var servicios = await _context.Servicios
                .Where(s => s.Clientes.Any(c => c.Id == clienteId) && !s.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ServicioDto>>(servicios);
        }

        // Obtener vehículos por ClienteId
        public async Task<IEnumerable<VehiculoDto>> GetVehiculosByClienteIdAsync(int clienteId)
        {
            var vehiculos = await _context.Vehiculos
                .Where(v => v.ClienteId == clienteId && !v.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
        }
    }
}
