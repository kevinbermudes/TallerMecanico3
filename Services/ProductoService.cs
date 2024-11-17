using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;

namespace TallerMecanico.Services
{
    public class ProductoService : IProductoService
    {
        private readonly TallerMecanicoContext _context;
        private readonly IMapper _mapper;

        public ProductoService(TallerMecanicoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los productos
        public async Task<IEnumerable<ProductoDto>> GetAllProductosAsync()
        {
            var productos = await _context.Productos
                .Include(p => p.Carritos)
                .Where(p => !p.EstaBorrado)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDto>>(productos);
        }

        // Obtener un producto específico por ID
        public async Task<ProductoDto> GetProductoByIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Carritos)
                .FirstOrDefaultAsync(p => p.Id == id && !p.EstaBorrado);

            if (producto == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            return _mapper.Map<ProductoDto>(producto);
        }

        // Crear un nuevo producto
        public async Task<ProductoDto> CreateProductoAsync(ProductoDto productoDto)
        {
            var producto = _mapper.Map<Producto>(productoDto);
            producto.FechaCreacion = DateTime.UtcNow;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductoDto>(producto);
        }

        // Actualizar un producto existente
        public async Task UpdateProductoAsync(int id, ProductoDto productoDto)
        {
            var producto = await _context.Productos
                .Include(p => p.Carritos) 
                .FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            // Actualizar solo las propiedades necesarias
            producto.Nombre = productoDto.Nombre;
            producto.Descripcion = productoDto.Descripcion;
            producto.Precio = productoDto.Precio;
            producto.Stock = productoDto.Stock;
            producto.Imagen = productoDto.Imagen;

            // No modificar la colección de Carritos aquí

            await _context.SaveChangesAsync();
        }


        // Borrado lógico de un producto
        public async Task DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null || producto.EstaBorrado)
                throw new KeyNotFoundException("Producto no encontrado o ya ha sido eliminado.");

            producto.EstaBorrado = true;
            producto.FechaBorrado = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Obtener productos por categoría
        public async Task<IEnumerable<ProductoDto>> GetProductosByCategoriaAsync(CategoriaProducto categoria)
        {
            var productos = await _context.Productos
                .Where(p => p.Categoria == categoria && !p.EstaBorrado)
                .Include(p => p.Carritos)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDto>>(productos);
        }

        // Actualizar stock de un producto
        public async Task UpdateStockAsync(int id, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null || producto.EstaBorrado)
                throw new KeyNotFoundException("Producto no encontrado o ha sido eliminado.");

            producto.Stock = cantidad;
            producto.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
