// Controllers/ProductoController.cs
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;
using TallerMecanico.models;
using Microsoft.AspNetCore.Http; // Necesario para IFormFile
using System.IO;
using System;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly IS3Service _s3Service;
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<ProductoController> _logger;


        public ProductoController(IProductoService productoService, IS3Service s3Service,IAmazonS3 s3Client,IConfiguration configuration, ILogger<ProductoController> logger)
        {
            _productoService = productoService;
            _s3Service = s3Service;
            _configuration = configuration;
            _s3Client = s3Client;
            _logger = logger;

        }

        // Obtener todos los productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAllProductos()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }

        // Obtener un producto específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProductoById(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                return Ok(producto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo producto
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> CreateProducto([FromForm] ProductoDto productoDto, [FromForm] IFormFile imagen)
        {
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    // Validar el archivo si es necesario

                    // Validar el tipo y tamaño del archivo
                    var tiposPermitidos = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                    if (!tiposPermitidos.Contains(extension))
                        return BadRequest("Tipo de archivo no permitido. Solo se permiten imágenes (jpg, jpeg, png, gif).");

                    if (imagen.Length > 5 * 1024 * 1024) // 5 MB como ejemplo
                        return BadRequest("El archivo es demasiado grande. El tamaño máximo permitido es 5 MB.");

                    // Generar un nombre de archivo único
                    string nombreArchivo = $"{Guid.NewGuid()}{extension}";

                    string bucketName = "tu-bucket-s3"; // Reemplaza con el nombre real de tu bucket

                    // Subir la imagen a S3
                    string url = await _s3Service.UploadFileAsync(imagen.OpenReadStream(), nombreArchivo, bucketName);

                    // Asignar la URL de la imagen al producto
                    productoDto.Imagen = url;
                }

                var nuevoProducto = await _productoService.CreateProductoAsync(productoDto);
                return CreatedAtAction(nameof(GetProductoById), new { id = nuevoProducto.Id }, nuevoProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el producto: {ex.Message}");
            }
        }

        // Actualizar un producto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromForm] ProductoDto productoDto, [FromForm] IFormFile imagen)
        {
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    // Validar el archivo si es necesario

                    // Validar el tipo y tamaño del archivo
                    var tiposPermitidos = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                    if (!tiposPermitidos.Contains(extension))
                        return BadRequest("Tipo de archivo no permitido. Solo se permiten imágenes (jpg, jpeg, png, gif).");

                    if (imagen.Length > 5 * 1024 * 1024) // 5 MB como ejemplo
                        return BadRequest("El archivo es demasiado grande. El tamaño máximo permitido es 5 MB.");

                    // Generar un nombre de archivo único
                    string nombreArchivo = $"{Guid.NewGuid()}{extension}";

                    string bucketName = "tu-bucket-s3"; // Reemplaza con el nombre real de tu bucket

                    // Subir la imagen a S3
                    string url = await _s3Service.UploadFileAsync(imagen.OpenReadStream(), nombreArchivo, bucketName);

                    // Asignar la URL de la imagen al producto
                    productoDto.Imagen = url;
                }

                await _productoService.UpdateProductoAsync(id, productoDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el producto: {ex.Message}");
            }
        }

        // Borrado lógico de un producto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                await _productoService.DeleteProductoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener productos por categoría
        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByCategoria(CategoriaProducto categoria)
        {
            var productos = await _productoService.GetProductosByCategoriaAsync(categoria);
            return Ok(productos);
        }

        // Actualizar stock de un producto
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int cantidad)
        {
            try
            {
                await _productoService.UpdateStockAsync(id, cantidad);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Endpoint para subir una imagen (redundante si ya se maneja en Create y Update)
         [HttpPost("{productoId}/imagen")]
        public async Task<IActionResult> UploadImage(int productoId, IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
                return BadRequest("Archivo no seleccionado.");

            try
            {
                var bucketName = _configuration["AWS:BucketName"]?.Trim();
                var region = _configuration["AWS:Region"]?.Trim();
                var key = $"productos/{productoId}/{imagen.FileName}";

                _logger.LogInformation($"Intentando subir imagen al bucket: {bucketName} en la región: {region}");

                var transferUtility = new TransferUtility(_s3Client);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = imagen.OpenReadStream(),
                    Key = key,
                    BucketName = bucketName,
                    ContentType = imagen.ContentType,
                    // CannedACL is removed
                };

                await transferUtility.UploadAsync(uploadRequest);

                var url = $"https://{bucketName}.s3.{region}.amazonaws.com/{key}";

                _logger.LogInformation($"Imagen subida exitosamente: {url}");

                // Actualizar el producto con la nueva URL de la imagen
                var producto = await _productoService.GetProductoByIdAsync(productoId);
                if (producto != null)
                {
                    producto.Imagen = url;
                    await _productoService.UpdateProductoAsync(productoId, producto);
                }

                return Ok(new { Url = url });
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, "Error al subir el archivo a S3.");
                return StatusCode(500, $"Error al subir el archivo: {e.Message}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error inesperado al subir el archivo.");
                return StatusCode(500, $"Error al subir el archivo: {e.Message}");
            }
        }

        // Endpoint para obtener la URL de la imagen
        [HttpGet("{id}/imagen")]
        public async Task<IActionResult> ObtenerImagen(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                if (producto == null)
                    return NotFound("Producto no encontrado.");

                if (string.IsNullOrEmpty(producto.Imagen))
                    return NotFound("El producto no tiene una imagen asignada.");

                return Ok(new { Url = producto.Imagen });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la imagen: {ex.Message}");
            }
        }
    }
}
