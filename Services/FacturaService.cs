using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Hubs;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public FacturaService(IHubContext<NotificationHub> hubContext,TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // Obtener todas las facturas
        public async Task<IEnumerable<FacturaDto>> GetAllFacturasAsync()
        {
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.CartasPago)
                .Where(f => !f.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FacturaDto>>(facturas);
        }

        // Obtener una factura específica por ID
        public async Task<FacturaDto> GetFacturaByIdAsync(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.CartasPago)
                .FirstOrDefaultAsync(f => f.Id == id && !f.EstaBorrado);

            if (factura == null)
                throw new KeyNotFoundException("Factura no encontrada.");

            return _mapper.Map<FacturaDto>(factura);
        }

        // Crear una nueva factura
        public async Task<FacturaDto> CreateFacturaAsync(FacturaDto facturaDto)
        {
            var factura = _mapper.Map<Factura>(facturaDto);
            factura.FechaCreacion = DateTime.UtcNow;
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.User(facturaDto.ClienteId.ToString())
                .SendAsync("ReceiveNotification", "Nueva factura generada.");;
            return _mapper.Map<FacturaDto>(factura);
        }

        // Actualizar una factura existente
        public async Task UpdateFacturaAsync(int id, FacturaDto facturaDto)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null || factura.EstaBorrado)
                throw new KeyNotFoundException("Factura no encontrada o ha sido eliminada.");

            _mapper.Map(facturaDto, factura);
            factura.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de una factura
        public async Task DeleteFacturaAsync(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null || factura.EstaBorrado)
                throw new KeyNotFoundException("Factura no encontrada o ya ha sido eliminada.");

            factura.EstaBorrado = true;
            factura.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener facturas por cliente
        public async Task<IEnumerable<FacturaDto>> GetFacturasByClienteIdAsync(int clienteId)
        {
            var facturas = await _context.Facturas
                .Where(f => f.ClienteId == clienteId && !f.EstaBorrado)
                .Include(f => f.Cliente)
                .Include(f => f.CartasPago)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FacturaDto>>(facturas);
        }

        // Obtener facturas por estado
        public async Task<IEnumerable<FacturaDto>> GetFacturasByEstadoAsync(EstadoFactura estado)
        {
            var facturas = await _context.Facturas
                .Where(f => f.Estado == estado && !f.EstaBorrado)
                .Include(f => f.Cliente)
                .Include(f => f.CartasPago)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FacturaDto>>(facturas);
        }
    }
}
