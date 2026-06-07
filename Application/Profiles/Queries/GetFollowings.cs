using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetFollowings
{
    public enum FollowPredicate
    {
        Followers,
        Followings
    }

    public class Query : IRequest<Result<List<ProfileDto>>>
    {
        public FollowPredicate Predicate { get; set; } = FollowPredicate.Followers;
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<List<ProfileDto>>>
    {
        public async Task<Result<List<ProfileDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profiles = new List<ProfileDto>();

            switch (request.Predicate)
            {
                case FollowPredicate.Followers:
                    profiles = await context.UserFollowings.Where(x => x.Target.Id == request.UserId)
                        .Select(x => x.Observer)
                        .ProjectTo<ProfileDto>(mapper.ConfigurationProvider,
                            new { currentUserId = userAccessor.GetUserId() })
                        .ToListAsync(cancellationToken);
                    break;
                case FollowPredicate.Followings:
                    profiles = await context.UserFollowings.Where(x => x.Observer.Id == request.UserId)
                        .Select(x => x.Target)
                        .ProjectTo<ProfileDto>(mapper.ConfigurationProvider,
                            new { currentUserId = userAccessor.GetUserId() })
                        .ToListAsync(cancellationToken);
                    break;
            }

            return Result<List<ProfileDto>>.Success(profiles);
        }
    }
}