using Application.Activities.DTOs;
using Application.Core;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetProfileById
{
    public class Query : IRequest<Result<ProfileDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<ProfileDto>>
    {
        public async Task<Result<ProfileDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profile = await context.Users.ProjectTo<ProfileDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

            if (profile == null) return Result<ProfileDto>.Failure("Profile not found!", 404);

            return Result<ProfileDto>.Success(profile);
        }
    }
}
