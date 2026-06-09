using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivities
{
    private const int MaxPageSize = 50;

    public class Query : IRequest<Result<PagedList<GetActivityDto, DateTime?>>>
    {
        public DateTime? After { get; set; }
        public DateTime? Before { get; set; }

        private int _pageSize = 3;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
        : IRequestHandler<Query, Result<PagedList<GetActivityDto, DateTime?>>>
    {
        public async Task<Result<PagedList<GetActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = context.Activities.AsQueryable();
            bool isBackward = request.Before.HasValue;

            if (request.After.HasValue)
            {
                query = query.Where(x => x.Date > request.After.Value).OrderBy(x => x.Date);
            }
            else if (request.Before.HasValue)
            {
                query = query.Where(x => x.Date < request.Before.Value).OrderByDescending(x => x.Date);
            }
            else
            {
                query = query.OrderBy(x => x.Date);
            }

            var activities = await query
                .Take(request.PageSize + 1)
                .ProjectTo<GetActivityDto>(mapper.ConfigurationProvider, new { currentUserId = userAccessor.GetUserId() })
                .ToListAsync(cancellationToken);

            bool hasExtraRecord = activities.Count > request.PageSize;
            if (hasExtraRecord)
            {
                activities.RemoveAt(activities.Count - 1);
            }

            if (isBackward)
            {
                activities.Reverse();
            }

            DateTime? nextCursor = null;
            DateTime? prevCursor = null;

            if (activities.Count > 0)
            {
                if ((!isBackward && hasExtraRecord) || isBackward)
                {
                    nextCursor = activities.Last().Date;
                }

                if ((isBackward && hasExtraRecord) || request.After.HasValue)
                {
                    prevCursor = activities.First().Date;
                }
            }

            var pagedList = new PagedList<GetActivityDto, DateTime?>
            {
                Items = activities,
                NextCursor = nextCursor,
                PrevCursor = prevCursor
            };

            return Result<PagedList<GetActivityDto, DateTime?>>.Success(pagedList);
        }
    }
}