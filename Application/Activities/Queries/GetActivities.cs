using Application.Activities.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivities
{
    public class Query : IRequest<List<GetActivityDto>> { }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, List<GetActivityDto>>
    {
        public async Task<List<GetActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Activities.ProjectTo<GetActivityDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
