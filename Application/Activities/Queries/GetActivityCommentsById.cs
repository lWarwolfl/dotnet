using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityCommentsById
{
    public class Query : IRequest<Result<List<GetCommentDto>>>
    {
        public required string ActivityId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<GetCommentDto>>>
    {
        public async Task<Result<List<GetCommentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await context.Comments.Where(x => x.ActivityId == request.ActivityId).OrderByDescending(x => x.CreatedAt).ProjectTo<GetCommentDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);


            return Result<List<GetCommentDto>>.Success(comments);
        }
    }
}
