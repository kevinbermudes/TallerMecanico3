using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class ServicioService : IServicioService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public ServicioService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los servicios
        public async Task<IEnumerable<ServicioDto>> GetAllServiciosAsync()
        {
            var servicios = await _context.Servicios
                .Include(s => s.Clientes)
                .Include(s => s.Vehiculos)
                .Where(s => !s.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ServicioDto>>(servicios);
        }

        // Obtener un servicio específico por ID
        public async Task<ServicioDto> GetServicioByIdAsync(int id)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Clientes)
                .Include(s => s.Vehiculos)
                .FirstOrDefaultAsync(s => s.Id == id && !s.EstaBorrado);

            if (servicio == null)
                throw new KeyNotFoundException("Servicio no encontrado.");

            return _mapper.Map<ServicioDto>(servicio);
        }

        // Crear un nuevo servicio
        public async Task<ServicioDto> CreateServicioAsync(ServicioDto servicioDto)
        {
            var servicio = _mapper.Map<Servicio>(servicioDto);
            servicio.FechaCreacion = DateTime.UtcNow;
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return _mapper.Map<ServicioDto>(servicio);
        }

        // Actualizar un servicio existente
        public async Task UpdateServicioAsync(int id, ServicioDto servicioDto)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null || servicio.EstaBorrado)
                throw new KeyNotFoundException("Servicio no encontrado o ha sido eliminado.");

            _mapper.Map(servicioDto, servicio);
            servicio.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de un servicio
        public async Task DeleteServicioAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null || servicio.EstaBorrado)
                throw new KeyNotFoundException("Servicio no encontrado o ya ha sido eliminado.");

            servicio.EstaBorrado = true;
            servicio.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener servicios por cliente
        public async Task<IEnumerable<ServicioDto>> GetServiciosByClienteIdAsync(int clienteId)
        {
            var servicios = await _context.Servicios
                .Where(s => s.Clientes.Any(c => c.Id == clienteId) && !s.EstaBorrado)
                .Include(s => s.Clientes)
                .Include(s => s.Vehiculos)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ServicioDto>>(servicios);
        }

        // Agregar cliente a servicio
        public async Task AddClienteToServicioAsync(int servicioId, int clienteId)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Clientes)
                .FirstOrDefaultAsync(s => s.Id == servicioId && !s.EstaBorrado);

            var cliente = await _context.Clientes.FindAsync(clienteId);

            if (servicio == null)
                throw new KeyNotFoundException("Servicio no encontrado.");
            
            if (cliente == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            if (!servicio.Clientes.Contains(cliente))
            {
                servicio.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
        }

        // Eliminar cliente de servicio
        public async Task RemoveClienteFromServicioAsync(int servicioId, int clienteId)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Clientes)
                .FirstOrDefaultAsync(s => s.Id == servicioId && !s.EstaBorrado);

            if (servicio == null)
                throw new KeyNotFoundException("Servicio no encontrado.");

            var cliente = servicio.Clientes.FirstOrDefault(c => c.Id == clienteId);
            if (cliente != null)
            {
                servicio.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        // Agregar vehiculo a servicio
        public async Task AddVehiculoToServicioAsync(int servicioId, int vehiculoId)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Vehiculos)
                .FirstOrDefaultAsync(s => s.Id == servicioId && !s.EstaBorrado);

            var vehiculo = await _context.Vehiculos.FindAsync(vehiculoId);

            if (servicio == null)
                throw new KeyNotFoundException("Servicio no encontrado.");

            if (vehiculo == null)
                throw new KeyNotFoundException("Vehículo no encontrado.");

            if (!servicio.Vehiculos.Contains(vehiculo))
            {
                servicio.Vehiculos.Add(vehiculo);
                await _context.SaveChangesAsync();
            }
        }

        // Eliminar vehiculo de servicio
        public async Task RemoveVehiculoFromServicioAsync(int servicioId, int vehiculoId)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Vehiculos)
                .FirstOrDefaultAsync(s => s.Id == servicioId && !s.EstaBorrado);

            if (servicio == null)
                throw new KeyNotFoundException("Servicio no encontrado.");

            var vehiculo = servicio.Vehiculos.FirstOrDefault(v => v.Id == vehiculoId);
            if (vehiculo != null)
            {
                servicio.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();
            }
        }
        // Obtener servicios por vehículo
        public async Task<IEnumerable<ServicioDto>> GetServiciosByVehiculoIdAsync(int vehiculoId)
        {
            var servicios = await _context.Servicios
                .Where(s => s.Vehiculos.Any(v => v.Id == vehiculoId) && !s.EstaBorrado)
                .Include(s => s.Clientes)
                .Include(s => s.Vehiculos)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ServicioDto>>(servicios);
        }

    }
}
