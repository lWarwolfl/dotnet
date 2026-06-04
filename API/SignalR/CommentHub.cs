using Application.Activities.Commands;
using Application.Activities.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

class CommentHub(IMediator mediator) : Hub
{
    public async Task SendComment(AddComment.Command command)
    {
        var comment = await mediator.Send(command);

        await Clients.Group(command.ActivityId).SendAsync("ReceiveComment", comment.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activityId = httpContext?.Request.Query["ActivityId"];

        if (string.IsNullOrEmpty(activityId)) throw new HubException("No activity with this Id");

        await Groups.AddToGroupAsync(Context.ConnectionId, activityId!);

        var result = await mediator.Send(new GetActivityCommentsById.Query { ActivityId = activityId! });

        await Clients.Caller.SendAsync("LoadComments", result.Value);
    }
}