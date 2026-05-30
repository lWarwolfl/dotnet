
using Application.Interfaces;
using Application.Profiles.DTOs;
using Imagekit;
using Imagekit.Models.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace Infrastructure.Images;

public class ImageService : IImageService
{
    private readonly ImageKitClient _imagekit;

    public ImageService(IOptions<ImagekitSettings> config)
    {
        _imagekit = new ImageKitClient()
        {
            BaseUrl = config.Value.UrlEndpoint,
            PrivateKey = config.Value.PrivateKey,
        };
    }

    public async Task<string> DeleteImage(string publicId)
    {
        try
        {
            var deletionParams = new FileDeleteParams { FileID = publicId };

            await _imagekit.Files.Delete(deletionParams);

            return "ok";
        }
        catch
        {
            throw new Exception("Image deletion failed");
        }
    }

    public async Task<ImageUploadResultDto?> UploadImage(IFormFile file)
    {
        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new FileUploadParams()
            {
                File = stream,
                FileName = file.FileName,
                Folder = "dotnet_client"
            };

            var uploadResult = await _imagekit.Files.Upload(uploadParams);

            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.FileID) || string.IsNullOrEmpty(uploadResult.Url))
            {
                throw new Exception("Image upload failed");
            }

            return new ImageUploadResultDto
            {
                PublicId = uploadResult.FileID,
                Url = uploadResult.Url
            };
        }

        return null;
    }
}
