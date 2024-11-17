using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using TallerMecanico.Interface;

namespace TallerMecanico.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;

        public S3Service(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(fileStream, bucketName, fileName);
                string url = $"https://{bucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";
                return url;
            }
            catch (Exception ex)
            {
                // Manejar excepciones según sea necesario
                throw new Exception($"Error al subir el archivo: {ex.Message}");
            }
        }

        public async Task<Stream> GetFileAsync(string fileName, string bucketName)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                using var response = await _s3Client.GetObjectAsync(request);
                var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception ex)
            {
                // Manejar excepciones según sea necesario
                throw new Exception($"Error al obtener el archivo: {ex.Message}");
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName, string bucketName)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                var response = await _s3Client.DeleteObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                // Manejar excepciones según sea necesario
                throw new Exception($"Error al eliminar el archivo: {ex.Message}");
            }
        }
    }
}