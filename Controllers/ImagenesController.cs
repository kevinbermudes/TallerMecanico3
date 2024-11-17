// Controllers/ImagenesController.cs
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Interface;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenesController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly IProductoService _productoService;

        public ImagenesController(IS3Service s3Service, IProductoService productoService)
        {
            _s3Service = s3Service;
            _productoService = productoService;
        }

        // Endpoint para subir una imagen
        [HttpPost("SubirImagen")]
        public async Task<IActionResult> SubirImagen([FromForm] IFormFile imagen, [FromForm] int productoId)
        {
            if (imagen == null || imagen.Length == 0)
                return BadRequest("No se ha proporcionado un archivo.");

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

            try
            {
                // Subir la imagen a S3
                string url = await _s3Service.UploadFileAsync(imagen.OpenReadStream(), nombreArchivo, bucketName);

                // Obtener el producto
                var productoDto = await _productoService.GetProductoByIdAsync(productoId);
                if (productoDto == null)
                    return NotFound("Producto no encontrado.");

                // Actualizar la URL de la imagen en el producto
                productoDto.Imagen = url;
                await _productoService.UpdateProductoAsync(productoId, productoDto);

                return Ok(new { Url = url });
            }
            catch (Exception ex)
            {
                // Manejar excepciones según sea necesario
                return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
            }
        }

        // Endpoint para obtener la URL de la imagen
        [HttpGet("ObtenerImagen/{productoId}")]
        public async Task<IActionResult> ObtenerImagen(int productoId)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(productoId);
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
