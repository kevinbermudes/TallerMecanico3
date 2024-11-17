using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Hubs;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class ParteService : IParteService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ParteService(TallerMecanicoContext context, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // Obtener todos los partes
        public async Task<IEnumerable<ParteDto>> GetAllPartesAsync()
        {
            var partes = await _context.Partes
                .Include(p => p.Cliente)
                .Where(p => !p.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ParteDto>>(partes);
        }

        // Obtener un parte específico por ID
        public async Task<ParteDto> GetParteByIdAsync(int id)
        {
            var parte = await _context.Partes
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id && !p.EstaBorrado);

            if (parte == null)
                throw new KeyNotFoundException("Parte no encontrada.");

            return _mapper.Map<ParteDto>(parte);
        }

        // Crear un nuevo parte
        public async Task<ParteDto> CreateParteAsync(ParteDto parteDto)
        {
            var parte = _mapper.Map<Parte>(parteDto);
            parte.FechaCreacion = DateTime.UtcNow;
            _context.Partes.Add(parte);
            await _context.SaveChangesAsync();

            // Enviar notificación en tiempo real al cliente específico
            await _hubContext.Clients.User(parteDto.ClienteId.ToString())
                .SendAsync("ReceiveNotification", "Se ha añadido una nueva parte.");

            return _mapper.Map<ParteDto>(parte);
        }

        // Actualizar un parte existente
        public async Task UpdateParteAsync(int id, ParteDto parteDto)
        {
            var parte = await _context.Partes.FindAsync(id);

            if (parte == null || parte.EstaBorrado)
                throw new KeyNotFoundException("Parte no encontrada o ha sido eliminada.");

            _mapper.Map(parteDto, parte);
            parte.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de un parte
        public async Task DeleteParteAsync(int id)
        {
            var parte = await _context.Partes.FindAsync(id);

            if (parte == null || parte.EstaBorrado)
                throw new KeyNotFoundException("Parte no encontrada o ya ha sido eliminada.");

            parte.EstaBorrado = true;
            parte.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener partes por cliente
        public async Task<IEnumerable<ParteDto>> GetPartesByClienteIdAsync(int clienteId)
        {
            var partes = await _context.Partes
                .Where(p => p.ClienteId == clienteId && !p.EstaBorrado)
                .Include(p => p.Cliente)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ParteDto>>(partes);
        }
    }
}
