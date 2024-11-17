using AutoMapper;
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
            var cartaPago = _mapper.Map<CartaPago>(cartaPagoDto);
            cartaPago.FechaCreacion = DateTime.UtcNow;
            _context.CartasPago.Add(cartaPago);
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
                .Include(cp => cp.Cliente)
                .Include(cp => cp.Factura)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartaPagoDto>>(cartasPago);
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
    }
}
