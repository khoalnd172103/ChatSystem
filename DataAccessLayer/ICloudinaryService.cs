using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
