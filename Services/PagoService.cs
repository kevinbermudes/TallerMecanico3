using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class PagoService : IPagoService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public PagoService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los pagos
        public async Task<IEnumerable<PagoDto>> GetAllPagosAsync()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Cliente)
                .Include(p => p.Producto)
                .Include(p => p.Servicio)
                .Include(p => p.Parte)
                .Where(p => !p.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }

        // Obtener un pago específico por ID
        public async Task<PagoDto> GetPagoByIdAsync(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.Cliente)
                .Include(p => p.Producto)
                .Include(p => p.Servicio)
                .Include(p => p.Parte)
                .FirstOrDefaultAsync(p => p.Id == id && !p.EstaBorrado);

            if (pago == null)
                throw new KeyNotFoundException("Pago no encontrado.");

            return _mapper.Map<PagoDto>(pago);
        }

        // Crear un nuevo pago
        public async Task<PagoDto> CreatePagoAsync(PagoDto pagoDto)
        {
            var pago = _mapper.Map<Pago>(pagoDto);
            pago.FechaCreacion = DateTime.UtcNow;
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return _mapper.Map<PagoDto>(pago);
        }

        // Actualizar un pago existente
        public async Task UpdatePagoAsync(int id, PagoDto pagoDto)
        {
            var pago = await _context.Pagos.FindAsync(id);

            if (pago == null || pago.EstaBorrado)
                throw new KeyNotFoundException("Pago no encontrado o ha sido eliminado.");

            _mapper.Map(pagoDto, pago);
            pago.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de un pago
        public async Task DeletePagoAsync(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);

            if (pago == null || pago.EstaBorrado)
                throw new KeyNotFoundException("Pago no encontrado o ya ha sido eliminado.");

            pago.EstaBorrado = true;
            pago.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener pagos por cliente
        public async Task<IEnumerable<PagoDto>> GetPagosByClienteIdAsync(int clienteId)
        {
            var pagos = await _context.Pagos
                .Where(p => p.ClienteId == clienteId && !p.EstaBorrado)
                .Include(p => p.Cliente)
                .Include(p => p.Producto)
                .Include(p => p.Servicio)
                .Include(p => p.Parte)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }

        // Obtener pagos por tipo de pago
        public async Task<IEnumerable<PagoDto>> GetPagosByTipoPagoAsync(TipoPago tipoPago)
        {
            var pagos = await _context.Pagos
                .Where(p => p.TipoPago == tipoPago && !p.EstaBorrado)
                .Include(p => p.Cliente)
                .Include(p => p.Producto)
                .Include(p => p.Servicio)
                .Include(p => p.Parte)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }

        // Obtener pagos por producto
        public async Task<IEnumerable<PagoDto>> GetPagosByProductoIdAsync(int productoId)
        {
            var pagos = await _context.Pagos
                .Where(p => p.ProductoId == productoId && !p.EstaBorrado)
                .Include(p => p.Cliente)
                .Include(p => p.Producto)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }

        // Obtener pagos por servicio
        public async Task<IEnumerable<PagoDto>> GetPagosByServicioIdAsync(int servicioId)
        {
            var pagos = await _context.Pagos
                .Where(p => p.ServicioId == servicioId && !p.EstaBorrado)
                .Include(p => p.Cliente)
                .Include(p => p.Servicio)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }

        // Obtener pagos por parte
        public async Task<IEnumerable<PagoDto>> GetPagosByParteIdAsync(int parteId)
        {
            var pagos = await _context.Pagos
                .Where(p => p.ParteId == parteId && !p.EstaBorrado)
                .Include(p => p.Cliente)
                .Include(p => p.Parte)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PagoDto>>(pagos);
        }
    }
}
