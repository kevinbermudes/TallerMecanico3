using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Hubs;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly PaymentService _paymentService;
        private readonly ICartaPagoService _cartaPagoService;


        public CarritoService(
            PaymentService paymentService,
            TallerMecanicoContext context,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext,
            ICartaPagoService cartaPagoService)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _paymentService = paymentService;
            _cartaPagoService = cartaPagoService; // Asignar la dependencia inyectada
        }

        // Obtener todos los carritos
        public async Task<IEnumerable<CarritoDto>> GetAllCarritosAsync()
        {
            var carritos = await _context.Carritos
                .Include(c => c.Cliente)
                .Include(c => c.Producto)
                .Where(c => !c.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CarritoDto>>(carritos);
        }

        // Obtener un carrito específico por ID
        public async Task<CarritoDto> GetCarritoByIdAsync(int id)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.Id == id && !c.EstaBorrado);

            if (carrito == null)
                throw new KeyNotFoundException("Carrito no encontrado.");

            return _mapper.Map<CarritoDto>(carrito);
        }

        // Crear un nuevo carrito
        public async Task<CarritoDto> CreateCarritoAsync(CarritoDto carritoDto)
        {
            var carrito = _mapper.Map<Carrito>(carritoDto);
            carrito.FechaCreacion = DateTime.UtcNow;
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();

            return _mapper.Map<CarritoDto>(carrito);
        }

        // Actualizar un carrito existente
        public async Task UpdateCarritoAsync(int id, CarritoDto carritoDto)
        {
            var carrito = await _context.Carritos.FindAsync(id);

            if (carrito == null || carrito.EstaBorrado)
                throw new KeyNotFoundException("Carrito no encontrado o ha sido eliminado.");

            _mapper.Map(carritoDto, carrito);
            carrito.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Borrado lógico de un carrito
        public async Task DeleteCarritoAsync(int id)
        {
            var carrito = await _context.Carritos.FindAsync(id);

            if (carrito == null || carrito.EstaBorrado)
                throw new KeyNotFoundException("Carrito no encontrado o ya ha sido eliminado.");

            carrito.EstaBorrado = true;
            carrito.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener carritos por cliente
        public async Task<IEnumerable<CarritoDto>> GetCarritosByClienteIdAsync(int clienteId)
        {
            var carritos = await _context.Carritos
                .Where(c => c.ClienteId == clienteId && !c.EstaBorrado)
                .Include(c => c.Cliente)
                .Include(c => c.Producto) // Incluye datos del producto
                .Include(c => c.Servicio) // Incluye datos del servicio
                .ToListAsync();

            return _mapper.Map<IEnumerable<CarritoDto>>(carritos);
        }


        // Obtener carritos por producto
        public async Task<IEnumerable<CarritoDto>> GetCarritosByProductoIdAsync(int productoId)
        {
            var carritos = await _context.Carritos
                .Where(c => c.ProductoId == productoId && !c.EstaBorrado)
                .Include(c => c.Cliente)
                .Include(c => c.Producto)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CarritoDto>>(carritos);
        }
        public async Task<CarritoDto> AgregarAlCarritoAsync(CarritoAgregarDto carritoAgregarDto)
        {
            // Verificar si el producto existe y tiene stock suficiente
            var producto = await _context.Productos.FindAsync(carritoAgregarDto.ProductoId);
            if (producto == null)
            {
                throw new KeyNotFoundException("Producto no encontrado.");
            }

            if (producto.Stock < carritoAgregarDto.Cantidad)
            {
                throw new Exception("Stock insuficiente para agregar al carrito.");
            }

            // Crear el nuevo carrito
            var carrito = new Carrito
            {
                ClienteId = carritoAgregarDto.ClienteId,
                ProductoId = carritoAgregarDto.ProductoId,
                Cantidad = carritoAgregarDto.Cantidad,
                PrecioTotal = producto.Precio * carritoAgregarDto.Cantidad,
                FechaCreacion = DateTime.UtcNow
            };

            // Guardar en la base de datos
            _context.Carritos.Add(carrito);

            // Crear una notificación
            var notificacion = new Notificacion
            {
                ClienteId = carritoAgregarDto.ClienteId,
                Mensaje = $"Has agregado {carritoAgregarDto.Cantidad} unidad(es) de {producto.Nombre} al carrito.",
                Tipo = TipoNotificacion.Carrito,
                Leido = false,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();

            // Enviar notificación al cliente en tiempo real
            await _hubContext.Clients.User(carritoAgregarDto.ClienteId.ToString())
                .SendAsync("ReceiveNotification", notificacion.Mensaje);

            // Mapear y devolver el DTO del carrito creado
            return _mapper.Map<CarritoDto>(carrito);
        }

     
        public async Task<Carrito> AgregarAlCarritoServicioAsync(CarritoAgregarservicioDto carritoAgregarDto)
        {
            var servicio = await _context.Servicios.FindAsync(carritoAgregarDto.ServicioId);
            if (servicio == null)
            {
                throw new KeyNotFoundException("Servicio no encontrado.");
            }

            var carrito = new Carrito
            {
                ClienteId = carritoAgregarDto.ClienteId,
                ServicioId = carritoAgregarDto.ServicioId,
                Cantidad = 1, // Asume cantidad predeterminada para servicios
                PrecioTotal = servicio.Precio, // Usa el precio del servicio
                FechaCreacion = DateTime.UtcNow
            };

            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();

            return carrito;
        }
public async Task<Factura> ConfirmarPagoAsync(int clienteId, string paymentIntentId)
{
    // Verificar el estado del PaymentIntent
    var paymentIntent = await _paymentService.VerificarIntentoPagoAsync(paymentIntentId);

    if (paymentIntent == null || !paymentIntent.Status.Equals("succeeded", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("El pago no fue exitoso. Verifica el intento de pago.");
    }

    // Obtener ítems del carrito
    var carritos = await _context.Carritos
        .Where(c => c.ClienteId == clienteId && !c.EstaBorrado)
        .Include(c => c.Producto)
        .Include(c => c.Servicio)
        .ToListAsync();

    if (!carritos.Any())
    {
        throw new InvalidOperationException("No hay ítems en el carrito para procesar.");
    }

    decimal total = 0;

    // Crear una nueva factura
    var factura = new Factura
    {
        ClienteId = clienteId,
        CodigoFactura = Guid.NewGuid().ToString().Substring(0, 8),
        Total = 0,
        FechaCreacion = DateTime.UtcNow,
        FechaVencimiento = DateTime.UtcNow.AddDays(30), // Factura vence en 30 días
        Estado = EstadoFactura.Pagada, // Estado inicial como pagada
        ProductosFactura = new List<ProductoFactura>(),
        ServiciosFactura = new List<ServicioFactura>()
    };

    _context.Facturas.Add(factura);

    foreach (var carrito in carritos)
    {
        if (carrito.Producto != null)
        {
            total += carrito.Producto.Precio * carrito.Cantidad;

            // Reducir el stock del producto
            carrito.Producto.Stock -= carrito.Cantidad;

            // Agregar a ProductoFactura
            factura.ProductosFactura.Add(new ProductoFactura
            {
                FacturaId = factura.Id,
                ProductoId = carrito.Producto.Id,
                Cantidad = carrito.Cantidad,
                PrecioUnitario = carrito.Producto.Precio
            });
        }
        else if (carrito.Servicio != null)
        {
            total += carrito.Servicio.Precio;

            // Agregar a ServicioFactura
            factura.ServiciosFactura.Add(new ServicioFactura
            {
                FacturaId = factura.Id,
                ServicioId = carrito.Servicio.Id,
                Precio = carrito.Servicio.Precio
            });

            // Asignar servicio al cliente usando ClienteServicio
            _context.ClienteServicios.Add(new ClienteServicio
            {
                ClienteId = clienteId,
                ServicioId = carrito.Servicio.Id,
                FechaAsignacion = DateTime.UtcNow
            });
        }

        // Eliminar ítem del carrito
        _context.Carritos.Remove(carrito);
    }

    // Actualizar el total de la factura
    factura.Total = total;

    // Guardar cambios en la base de datos
    await _context.SaveChangesAsync();

    return factura;
}








    }
}
