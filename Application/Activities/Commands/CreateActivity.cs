using System;
using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
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
        public required CreateActivityDTO CreateActivityDTO { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = mapper.Map<Activity>(request.CreateActivityDTO);

            context.Activities.Add(activity);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Response>.Failure("Failed to create activity!", 400);

            return Result<Response>.Success(new Response { Id = activity.Id });
            ;
        }
    }
}
