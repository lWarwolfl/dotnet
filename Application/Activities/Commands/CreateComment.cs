using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Commands;

public class CreateComment
{

    public class Command : IRequest<Result<GetCommentDto>>
    {
        public required CreateCommentDto createCommentDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Command, Result<GetCommentDto>>
    {
        public async Task<Result<GetCommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.Include(x => x.Comments).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == request.createCommentDto.ActivityId, cancellationToken);

            if (activity == null) return Result<GetCommentDto>.Failure("Activity not found!", 404);

            var user = await userAccessor.GetUserAsync();

            var comment = new Comment { ActivityId = activity.Id, UserId = user.Id, Body = request.createCommentDto.Body };

            activity.Comments.Add(comment);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<GetCommentDto>.Failure("Failed to add comment!", 400);

            return Result<GetCommentDto>.Success(mapper.Map<GetCommentDto>(comment));
            ;
        }
    }
}
