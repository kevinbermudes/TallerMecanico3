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

        public FacturaService(IHubContext<NotificationHub> hubContext, TallerMecanicoContext context, IMapper mapper)
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
                .Include(f => f.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.CartaPago)
                .Where(f => !f.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FacturaDto>>(facturas);
        }

        // Obtener una factura específica por ID
        public async Task<FacturaDto> GetFacturaByIdAsync(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.CartaPago)
                .FirstOrDefaultAsync(f => f.Id == id && !f.EstaBorrado);

            if (factura == null)
                throw new KeyNotFoundException("Factura no encontrada.");

            return _mapper.Map<FacturaDto>(factura);
        }

        // Crear una nueva factura
public async Task<FacturaDto> CreateFacturaAsync(FacturaDto facturaDto)
{
    var factura = _mapper.Map<Factura>(facturaDto);

    // Generar un código único para la factura
    factura.CodigoFactura = Guid.NewGuid().ToString().Substring(0, 8);
    factura.FechaCreacion = DateTime.UtcNow;

    // Guardar la factura primero
    _context.Facturas.Add(factura);
    await _context.SaveChangesAsync(); // Factura.Id ahora es conocido

    // Si la factura es pendiente, crear o asociar una carta de pago
    if (factura.Estado == EstadoFactura.Pendiente)
    {
        // Buscar o crear una carta de pago para la misma fecha de vencimiento
        var cartaPago = await _context.CartasPago
            .FirstOrDefaultAsync(cp =>
                cp.ClienteId == factura.ClienteId &&
                cp.FechaPago.Date == factura.FechaVencimiento.Date &&
                !cp.EstaBorrado);

        if (cartaPago == null)
        {
            // Crear una nueva carta de pago
            cartaPago = new CartaPago
            {
                ClienteId = factura.ClienteId,
                FechaPago = factura.FechaVencimiento,
                Monto = factura.Total,
                MetodoPago = MetodoPago.Pasarela,
                FechaCreacion = DateTime.UtcNow
            };

            _context.CartasPago.Add(cartaPago);
            await _context.SaveChangesAsync(); // Guarda para generar CartaPago.Id
        }
        else
        {
            // Actualizar el monto de la carta de pago existente
            cartaPago.Monto += factura.Total;
            cartaPago.FechaActualizacion = DateTime.UtcNow;
            _context.CartasPago.Update(cartaPago);
            await _context.SaveChangesAsync();
        }

        // Asociar la factura a la carta de pago mediante la tabla relacional
        var facturaCartaPago = new FacturaCartaPago
        {
            FacturaId = factura.Id,
            CartaPagoId = cartaPago.Id
        };
        _context.FacturaCartaPagos.Add(facturaCartaPago);
    }

    // Guardar los cambios finales
    await _context.SaveChangesAsync();

    // Notificar al cliente asociado
    await _hubContext.Clients.User(factura.ClienteId.ToString())
        .SendAsync("ReceiveNotification", "Nueva factura generada.");

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
        public async Task<List<FacturaDto>> GetFacturasByClienteIdAsync(int clienteId)
        {
            var facturas = await _context.Facturas
                .Where(f => f.ClienteId == clienteId && !f.EstaBorrado)
                .Include(f => f.ProductosFactura)
                .ThenInclude(pf => pf.Producto)
                .Include(f => f.ServiciosFactura)
                .ThenInclude(sf => sf.Servicio)
                .ToListAsync();

            return _mapper.Map<List<FacturaDto>>(facturas);
        }

        // Obtener facturas por estado
        public async Task<IEnumerable<FacturaDto>> GetFacturasByEstadoAsync(EstadoFactura estado)
        {
            var facturas = await _context.Facturas
                .Where(f => f.Estado == estado && !f.EstaBorrado)
                .Include(f => f.Cliente)
                .Include(f => f.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.CartaPago)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FacturaDto>>(facturas);
        }
    }
}
