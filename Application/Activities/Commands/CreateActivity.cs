using System;
using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Response
    {
        public required string Id { get; set; }
    }

    public class Command : IRequest<Result<Response>>
    {
        public required CreateActivityDto ActivityDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserAsync();

            var activity = mapper.Map<Activity>(request.ActivityDto);

            context.Activities.Add(activity);

            var attendee = new ActivityAttendee { ActivityId = activity.Id, UserId = user.Id, IsHost = true };

            activity.Attendees.Add(attendee);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Response>.Failure("Failed to create activity!", 400);

            return Result<Response>.Success(new Response { Id = activity.Id });
            ;
        }
    }
}
