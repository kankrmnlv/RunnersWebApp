using CloudinaryDotNet.Actions;

namespace RunnersWebApp.Interfaces
{
    public interface IPhotoInterface
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile photoFile);
        Task<DeletionResult> DeletePhotoAsync(string photoId);
    }
}
