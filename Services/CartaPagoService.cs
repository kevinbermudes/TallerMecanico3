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
                .Include(cp => cp.Factura)
                .Where(cp => !cp.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
        }

        // Obtener una carta de pago específica por ID
        public async Task<CartaPagoDto> GetCartaPagoByIdAsync(int id)
        {
            var cartaPago = await _context.CartasPago
                .Include(cp => cp.Cliente)
                .Include(cp => cp.Factura)
                .FirstOrDefaultAsync(cp => cp.Id == id && !cp.EstaBorrado);

            if (cartaPago == null)
                throw new KeyNotFoundException("Carta de pago no encontrada.");

            return _mapper.Map<CartaPagoDto>(cartaPago);
        }

        // Crear una nueva carta de pago
        public async Task<CartaPagoDto> CreateCartaPagoAsync(CartaPagoDto cartaPagoDto)
        {
            // Validar que la factura exista y esté pendiente
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.Id == cartaPagoDto.FacturaId);

            if (factura == null)
                throw new KeyNotFoundException("Factura no encontrada.");

            if (factura.Estado != EstadoFactura.Pendiente)
                throw new InvalidOperationException("No se puede crear una carta de pago para una factura que ya está pagada.");

            // Verificar si ya existe una carta de pago para la misma factura y fecha de vencimiento
            var existingCartaPago = await _context.CartasPago
                .FirstOrDefaultAsync(cp => cp.FacturaId == cartaPagoDto.FacturaId && cp.FechaPago.Date == cartaPagoDto.FechaPago.Date);

            if (existingCartaPago != null)
            {
                throw new InvalidOperationException("Ya existe una carta de pago para esta factura y fecha de vencimiento.");
            }

            // Crear la carta de pago
            var cartaPago = _mapper.Map<CartaPago>(cartaPagoDto);
            cartaPago.FechaCreacion = DateTime.UtcNow;

            _context.CartasPago.Add(cartaPago);

            // Actualizar el estado de la factura si corresponde
            var totalPagado = _context.CartasPago
                .Where(cp => cp.FacturaId == cartaPago.FacturaId && !cp.EstaBorrado)
                .Sum(cp => cp.Monto);

            if (totalPagado + cartaPago.Monto >= factura.Total)
            {
                factura.Estado = EstadoFactura.Pagada;
                factura.FechaActualizacion = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<CartaPagoDto>(cartaPago);
        }



        // Actualizar una carta de pago existente
        public async Task UpdateCartaPagoAsync(int id, CartaPagoDto cartaPagoDto)
        {
            var cartaPago = await _context.CartasPago.FindAsync(id);

            if (cartaPago == null || cartaPago.EstaBorrado)
                throw new KeyNotFoundException("Carta de pago no encontrada o ha sido eliminada.");

            _mapper.Map(cartaPagoDto, cartaPago);
            cartaPago.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de una carta de pago
        public async Task DeleteCartaPagoAsync(int id)
        {
            var cartaPago = await _context.CartasPago.FindAsync(id);

            if (cartaPago == null || cartaPago.EstaBorrado)
                throw new KeyNotFoundException("Carta de pago no encontrada o ya ha sido eliminada.");

            cartaPago.EstaBorrado = true;
            cartaPago.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener cartas de pago por cliente
        public async Task<IEnumerable<CartaPagoDto>> GetCartasPagoByClienteIdAsync(int clienteId)
        {
            var cartasPago = await _context.CartasPago
                .Where(cp => cp.ClienteId == clienteId && !cp.EstaBorrado)
                .Include(cp => cp.Factura)
                .ThenInclude(f => f.ProductosFactura) // Incluye los productos
                .Include(cp => cp.Factura)
                .ThenInclude(f => f.ServiciosFactura) // Incluye los servicios
                .ToListAsync();

            // Calcula el monto total de cada carta de pago
            foreach (var carta in cartasPago)
            {
                carta.Monto = carta.Factura.ProductosFactura.Sum(pf => pf.PrecioUnitario * pf.Cantidad)
                              + carta.Factura.ServiciosFactura.Sum(sf => sf.Precio);
            }

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
        }



        public Task<ActionResult<IEnumerable<FacturaDto>>> GetFacturasByCartaPagoId(int id)
        {
            throw new NotImplementedException();
        }


        // Obtener cartas de pago por factura
        public async Task<IEnumerable<CartaPagoDto>> GetCartasPagoByFacturaIdAsync(int facturaId)
        {
            var cartasPago = await _context.CartasPago
                .Where(cp => cp.FacturaId == facturaId && !cp.EstaBorrado)
                .Include(cp => cp.Cliente)
                .Include(cp => cp.Factura)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
        }
        public async Task<IEnumerable<FacturaDto>> GetFacturasByCartaPagoIdAsync(int cartaPagoId)
        {
            var cartaPago = await _context.CartasPago
                .Include(cp => cp.Factura)
                .ThenInclude(f => f.ProductosFactura)
                .Include(cp => cp.Factura)
                .ThenInclude(f => f.ServiciosFactura)
                .FirstOrDefaultAsync(cp => cp.Id == cartaPagoId && !cp.EstaBorrado);

            if (cartaPago == null)
                throw new KeyNotFoundException("Carta de pago no encontrada.");

            // Retornar la factura asociada
            return new List<Factura> { cartaPago.Factura }
                .Select(f => _mapper.Map<FacturaDto>(f));
        }

        public async Task<CartaPagoDto> CreateOrUpdateCartaPagoAsync(int clienteId, DateTime fechaVencimiento)
        {
            // Buscar si ya existe una carta de pago para la fecha de vencimiento y cliente
            var existingCartaPago = await _context.CartasPago
                .FirstOrDefaultAsync(cp => cp.ClienteId == clienteId && cp.FechaPago.Date == fechaVencimiento.Date);

            if (existingCartaPago == null)
            {
                // Crear una nueva carta de pago si no existe
                existingCartaPago = new CartaPago
                {
                    ClienteId = clienteId,
                    FechaPago = fechaVencimiento,
                    Monto = 0, // Calcularemos el monto más adelante
                    MetodoPago = MetodoPago.Pasarela,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.CartasPago.Add(existingCartaPago);
            }

            // Buscar facturas pendientes con la misma fecha de vencimiento
            var facturasPendientes = await _context.Facturas
                .Where(f => f.ClienteId == clienteId && f.FechaVencimiento.Date == fechaVencimiento.Date && f.Estado == EstadoFactura.Pendiente)
                .ToListAsync();

            // Actualizar el monto de la carta de pago
            existingCartaPago.Monto = facturasPendientes.Sum(f => f.Total);

            // Cambiar estado de las facturas a pagadas
            foreach (var factura in facturasPendientes)
            {
                factura.Estado = EstadoFactura.Pagada;
                factura.FechaActualizacion = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<CartaPagoDto>(existingCartaPago);
        }

        

    }
}
