using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Dtos;
using TallerMecanico.Interface;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;
        private readonly IS3Service _s3Service;
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<ServicioController> _logger;


        public ServicioController(
            IServicioService servicioService,
            IS3Service s3Service,
            IAmazonS3 s3Client,
            IConfiguration configuration,
            ILogger<ServicioController> logger)
        {
            _servicioService = servicioService;
            _s3Service = s3Service;
            _configuration = configuration;
            _s3Client = s3Client;
            _logger = logger;
        }

        // Obtener todos los servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetAllServicios()
        {
            var servicios = await _servicioService.GetAllServiciosAsync();
            return Ok(servicios);
        }

        // Obtener un servicio específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> GetServicioById(int id)
        {
            try
            {
                var servicio = await _servicioService.GetServicioByIdAsync(id);
                return Ok(servicio);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Crear un nuevo servicio
        // [HttpPost]
        // public async Task<ActionResult<ServicioDto>> CreateServicio(ServicioDto servicioDto)
        // {
        //     var nuevoServicio = await _servicioService.CreateServicioAsync(servicioDto);
        //     return CreatedAtAction(nameof(GetServicioById), new { id = nuevoServicio.Id }, nuevoServicio);
        // }

        // Actualizar un servicio existente
        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateServicio(int id, ServicioDto servicioDto)
        // {
        //     try
        //     {
        //         await _servicioService.UpdateServicioAsync(id, servicioDto);
        //         return NoContent();
        //     }
        //     catch (KeyNotFoundException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        // }

        // Borrado lógico de un servicio
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                await _servicioService.DeleteServicioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Obtener servicios por cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServiciosByClienteId(int clienteId)
        {
            var servicios = await _servicioService.GetServiciosByClienteIdAsync(clienteId);
            return Ok(servicios);
        }

        // Agregar cliente a un servicio
        [HttpPost("{servicioId}/cliente/{clienteId}")]
        public async Task<IActionResult> AddClienteToServicio(int servicioId, int clienteId)
        {
            try
            {
                await _servicioService.AddClienteToServicioAsync(servicioId, clienteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("contratar")]
        public async Task<IActionResult> ContratarServicio([FromBody] ContratarServicioDto contratarDto)
        {
            try
            {
                await _servicioService.AddClienteToServicioAsync(contratarDto.ServicioId, contratarDto.ClienteId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al contratar el servicio: {ex.Message}");
            }
        }
        [HttpPost("{servicioId}/imagen")]
        public async Task<IActionResult> UploadImage(int servicioId, IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
                return BadRequest("Archivo no seleccionado.");

            try
            {
                var bucketName = _configuration["AWS:BucketName"]?.Trim();
                var region = _configuration["AWS:Region"]?.Trim();
                var key = $"servicios/{servicioId}/{imagen.FileName}";

                _logger.LogInformation($"Intentando subir imagen al bucket: {bucketName} en la región: {region}");

                var transferUtility = new TransferUtility(_s3Client);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = imagen.OpenReadStream(),
                    Key = key,
                    BucketName = bucketName,
                    ContentType = imagen.ContentType,
                };

                await transferUtility.UploadAsync(uploadRequest);

                var url = $"https://{bucketName}.s3.{region}.amazonaws.com/{key}";

                _logger.LogInformation($"Imagen subida exitosamente: {url}");

                // Actualizar el servicio con la nueva URL de la imagen
                var servicio = await _servicioService.GetServicioByIdAsync(servicioId);
                if (servicio != null)
                {
                    servicio.Imagen = url;
                    await _servicioService.UpdateServicioAsync(servicioId, servicio);
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
        [HttpGet("{id}/imagen")]
        public async Task<IActionResult> ObtenerImagen(int id)
        {
            try
            {
                var servicio = await _servicioService.GetServicioByIdAsync(id);
                if (servicio == null)
                    return NotFound("Servicio no encontrado.");

                if (string.IsNullOrEmpty(servicio.Imagen))
                    return NotFound("El servicio no tiene una imagen asignada.");

                return Ok(new { Url = servicio.Imagen });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la imagen: {ex.Message}");
            }
        }
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateServicio(int id, [FromForm] ServicioDto servicioDto, [FromForm] IFormFile? imagen)
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

                        string bucketName = _configuration["AWS:BucketName"]?.Trim();

                        // Subir la imagen a S3
                        string url = await _s3Service.UploadFileAsync(imagen.OpenReadStream(), nombreArchivo, bucketName);

                        // Asignar la URL de la imagen al servicio
                        servicioDto.Imagen = url;
                    }

                    await _servicioService.UpdateServicioAsync(id, servicioDto);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, $"Error al actualizar el servicio: {ex.Message}");
                }
            }

        [HttpPost]
        public async Task<ActionResult<ServicioDto>> CreateServicio([FromForm] ServicioDto servicioDto, [FromForm] IFormFile? imagen)
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

                    string bucketName = _configuration["AWS:BucketName"]?.Trim();

                    // Subir la imagen a S3
                    string url = await _s3Service.UploadFileAsync(imagen.OpenReadStream(), nombreArchivo, bucketName);

                    // Asignar la URL de la imagen al servicio
                    servicioDto.Imagen = url;
                }

                var nuevoServicio = await _servicioService.CreateServicioAsync(servicioDto);
                return CreatedAtAction(nameof(GetServicioById), new { id = nuevoServicio.Id }, nuevoServicio);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Error al crear el servicio: {ex.Message}");
            }
        }
        [HttpDelete("{servicioId}/cliente/{clienteId}")]
        public async Task<IActionResult> RemoveServicioFromCliente(int servicioId, int clienteId)
        {
            try
            {
                await _servicioService.RemoveClienteFromServicioAsync(servicioId, clienteId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el servicio: {ex.Message}");
            }
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> ReactivarServicio(int id)
        {
            try
            {
                await _servicioService.ReactivarServicioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al reactivar el servicio: {ex.Message}");
            }
        }



    }
}
