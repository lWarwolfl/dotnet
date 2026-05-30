using System;
using Application.Profiles.DTOs;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IImageService
{
    Task<ImageUploadResultDto?> UploadImage(IFormFile file);
    Task<string> DeleteImage(string publicId);
}
