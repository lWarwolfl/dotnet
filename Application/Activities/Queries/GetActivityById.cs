using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityById
{
    public class Query : IRequest<Result<GetActivityDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<GetActivityDto>>
    {
        public async Task<Result<GetActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.ProjectTo<GetActivityDto>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId() }).FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

            if (activity == null) return Result<GetActivityDto>.Failure("Activity not found!", 404);

            return Result<GetActivityDto>.Success(activity);
        }
    }
}
