
namespace TallerMecanico.Interface
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string bucketName);
        Task<Stream> GetFileAsync(string fileName, string bucketName);
        Task<bool> DeleteFileAsync(string fileName, string bucketName);
    }
}