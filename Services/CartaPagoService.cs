using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class CartaPagoService : ICartaPagoService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public CartaPagoService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todas las cartas de pago
        public async Task<IEnumerable<CartaPagoDto>> GetAllCartasPagoAsync()
        {
            var cartasPago = await _context.CartasPago
                .Include(cp => cp.Cliente)
                .Include(cp => cp.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.Factura)
                .Where(cp => !cp.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
        }

        // Obtener una carta de pago específica por ID
        public async Task<CartaPagoDto> GetCartaPagoByIdAsync(int id)
        {
            var cartaPago = await _context.CartasPago
                .Include(cp => cp.Cliente)
                .Include(cp => cp.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.Factura)
                .FirstOrDefaultAsync(cp => cp.Id == id && !cp.EstaBorrado);

            if (cartaPago == null)
                throw new KeyNotFoundException("Carta de pago no encontrada.");

            return _mapper.Map<CartaPagoDto>(cartaPago);
        }

        // Crear o actualizar una carta de pago
        public async Task<CartaPagoDto> CreateOrUpdateCartaPagoAsync(int clienteId, DateTime fechaVencimiento)
        {
            // Buscar o crear una carta de pago
            var existingCartaPago = await _context.CartasPago
                .Include(cp => cp.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.Factura)
                .FirstOrDefaultAsync(cp => cp.ClienteId == clienteId && cp.FechaPago.Date == fechaVencimiento.Date && !cp.EstaBorrado);

            if (existingCartaPago == null)
            {
                existingCartaPago = new CartaPago
                {
                    ClienteId = clienteId,
                    FechaPago = fechaVencimiento,
                    Monto = 0,
                    MetodoPago = MetodoPago.Pasarela,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.CartasPago.Add(existingCartaPago);
            }

            // Buscar facturas pendientes con la misma fecha de vencimiento
            var facturasPendientes = await _context.Facturas
                .Where(f => f.ClienteId == clienteId && f.FechaVencimiento.Date == fechaVencimiento.Date && f.Estado == EstadoFactura.Pendiente)
                .ToListAsync();

            foreach (var factura in facturasPendientes)
            {
                // Asociar las facturas pendientes a la carta de pago si no están ya asociadas
                if (!existingCartaPago.FacturaCartaPagos.Any(fcp => fcp.FacturaId == factura.Id))
                {
                    existingCartaPago.FacturaCartaPagos.Add(new FacturaCartaPago
                    {
                        FacturaId = factura.Id,
                        CartaPagoId = existingCartaPago.Id
                    });
                }
            }

            // Actualizar el monto de la carta de pago
            existingCartaPago.Monto = existingCartaPago.FacturaCartaPagos.Sum(fcp => fcp.Factura.Total);

            await _context.SaveChangesAsync();

            return _mapper.Map<CartaPagoDto>(existingCartaPago);
        }

        // Eliminar una carta de pago lógicamente si no tiene facturas pendientes
        public async Task DeleteCartaPagoIfNoPendingInvoicesAsync(int cartaPagoId)
        {
            var cartaPago = await _context.CartasPago
                .Include(cp => cp.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.Factura)
                .FirstOrDefaultAsync(cp => cp.Id == cartaPagoId && !cp.EstaBorrado);

            if (cartaPago == null)
                throw new KeyNotFoundException("Carta de pago no encontrada.");

            // Verificar si todas las facturas asociadas están pagadas
            var tieneFacturasPendientes = cartaPago.FacturaCartaPagos
                .Any(fcp => fcp.Factura.Estado == EstadoFactura.Pendiente);

            if (!tieneFacturasPendientes)
            {
                cartaPago.EstaBorrado = true;
                cartaPago.FechaBorrado = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        // Obtener cartas de pago por cliente
        public async Task<IEnumerable<CartaPagoDto>> GetCartasPagoByClienteIdAsync(int clienteId)
        {
            var cartasPago = await _context.CartasPago
                .Where(cp => cp.ClienteId == clienteId && !cp.EstaBorrado)
                .Include(cp => cp.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.Factura)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
        }
        public async Task<IEnumerable<FacturaDto>> GetFacturasByCartaPagoIdAsync(int cartaPagoId)
        {
            // Buscar la carta de pago con sus facturas asociadas
            var cartaPago = await _context.CartasPago
                .Include(cp => cp.FacturaCartaPagos)
                .ThenInclude(fcp => fcp.Factura)
                .FirstOrDefaultAsync(cp => cp.Id == cartaPagoId && !cp.EstaBorrado);

            if (cartaPago == null)
                throw new KeyNotFoundException("Carta de pago no encontrada.");

            // Filtrar las facturas en estado Pendiente
            var facturasPendientes = cartaPago.FacturaCartaPagos
                .Where(fcp => fcp.Factura.Estado == EstadoFactura.Pendiente)
                .Select(fcp => fcp.Factura);

            // Mapear las facturas pendientes a DTOs
            return _mapper.Map<IEnumerable<FacturaDto>>(facturasPendientes);
        }


    }
}
