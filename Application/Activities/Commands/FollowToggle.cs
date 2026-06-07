using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Commands;

public class FollowToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string TargetUserID { get; set; }
    }

    public class Handler(IUserAccessor userAccessor, AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await userAccessor.GetUserAsync();
            var target = await context.Users.FindAsync([request.TargetUserID], cancellationToken);

            if (target == null) return Result<Unit>.Failure("Target user not found!", 400);

            var following = await context.UserFollowings.FindAsync([observer.Id, target.Id], cancellationToken);

            if (following == null)
            {
                context.UserFollowings.Add(new UserFollowing { ObserverId = observer.Id, TargetId = target.Id });
            }
            else
            {
                context.UserFollowings.Remove(following);
            }

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to toggle follow!", 400);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
