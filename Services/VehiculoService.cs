using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public VehiculoService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los vehículos
        public async Task<IEnumerable<VehiculoDto>> GetAllVehiculosAsync()
        {
            var vehiculos = await _context.Vehiculos
                .Include(v => v.Cliente)
                .Include(v => v.ServiciosRealizados)
                .Where(v => !v.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
        }

        // Obtener un vehículo específico por ID
        public async Task<VehiculoDto> GetVehiculoByIdAsync(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Cliente)
                .Include(v => v.ServiciosRealizados)
                .FirstOrDefaultAsync(v => v.Id == id && !v.EstaBorrado);

            if (vehiculo == null)
                throw new KeyNotFoundException("Vehículo no encontrado.");

            return _mapper.Map<VehiculoDto>(vehiculo);
        }

        // Crear un nuevo vehículo
        public async Task<VehiculoDto> CreateVehiculoAsync(VehiculoDto vehiculoDto)
        {
            var vehiculo = _mapper.Map<Vehiculo>(vehiculoDto);
            vehiculo.FechaCreacion = DateTime.UtcNow;
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehiculoDto>(vehiculo);
        }

        // Actualizar un vehículo existente
        public async Task UpdateVehiculoAsync(int id, VehiculoDto vehiculoDto)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);

            if (vehiculo == null || vehiculo.EstaBorrado)
                throw new KeyNotFoundException("Vehículo no encontrado o ha sido eliminado.");

            _mapper.Map(vehiculoDto, vehiculo);
            vehiculo.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de un vehículo
        public async Task DeleteVehiculoAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);

            if (vehiculo == null || vehiculo.EstaBorrado)
                throw new KeyNotFoundException("Vehículo no encontrado o ya ha sido eliminado.");

            vehiculo.EstaBorrado = true;
            vehiculo.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener vehículos por cliente
        public async Task<IEnumerable<VehiculoDto>> GetVehiculosByClienteIdAsync(int clienteId)
        {
            var vehiculos = await _context.Vehiculos
                .Where(v => v.ClienteId == clienteId && !v.EstaBorrado)
                .Include(v => v.Cliente)
                .Include(v => v.ServiciosRealizados)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VehiculoDto>>(vehiculos);
        }

        // Agregar servicio a vehículo
        public async Task AddServicioToVehiculoAsync(int vehiculoId, int servicioId)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.ServiciosRealizados)
                .FirstOrDefaultAsync(v => v.Id == vehiculoId && !v.EstaBorrado);

            var servicio = await _context.Servicios.FindAsync(servicioId);

            if (vehiculo == null)
                throw new KeyNotFoundException("Vehículo no encontrado.");

            if (servicio == null)
                throw new KeyNotFoundException("Servicio no encontrado.");

            if (!vehiculo.ServiciosRealizados.Contains(servicio))
            {
                vehiculo.ServiciosRealizados.Add(servicio);
                await _context.SaveChangesAsync();
            }
        }

        // Eliminar servicio de vehículo
        public async Task RemoveServicioFromVehiculoAsync(int vehiculoId, int servicioId)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.ServiciosRealizados)
                .FirstOrDefaultAsync(v => v.Id == vehiculoId && !v.EstaBorrado);

            if (vehiculo == null)
                throw new KeyNotFoundException("Vehículo no encontrado.");

            var servicio = vehiculo.ServiciosRealizados.FirstOrDefault(s => s.Id == servicioId);
            if (servicio != null)
            {
                vehiculo.ServiciosRealizados.Remove(servicio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
