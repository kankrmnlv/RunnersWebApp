using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunnersWebApp.Helpers;
using RunnersWebApp.Interfaces;

namespace RunnersWebApp.Services
{
    public class PhotoService : IPhotoInterface
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile photoFile)
        {
            var uploadResult = new ImageUploadResult();
            if (photoFile.Length > 0)
            {
                using var stream = photoFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photoFile.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string photoId)
        {
            var deleteParams = new DeletionParams(photoId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}
