using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Profiles.Commands;

public class UpdateImage
{
    public class Command : IRequest<Result<Image>>
    {
        public required IFormFile File { get; set; }
    }

    public class Handler(AppDbContext context, IImageService imageService, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Image>>
    {
        public async Task<Result<Image>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserWithImagesAsync();

            var oldImage = user.Images.FirstOrDefault();

            if (oldImage != null)
            {
                var deletionResult = await imageService.DeleteImage(oldImage.PublicId);

                if (deletionResult == "ok") context.Images.Remove(oldImage);
            }

            var uploadResult = await imageService.UploadImage(request.File);

            if (uploadResult == null) return Result<Image>.Failure("Failed to upload image", 400);

            var image = new Image { PublicId = uploadResult.PublicId, Url = uploadResult.Url, UserId = user.Id };

            user.ImageUrl = image.Url;

            context.Images.Add(image);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<Image>.Success(image)
                : Result<Image>.Failure("Failed to update image", 400);
        }
    }
}
