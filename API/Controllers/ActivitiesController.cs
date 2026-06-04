using Application.Activities.Commands;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<GetActivityDto>>> ActivitiesGet()
    {
        return await Mediator.Send(new GetActivities.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetActivityDto>> ActivityByIdGet(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityById.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<CreateActivity.Response>> ActivityPost(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "IsActivityHost")]
    public async Task<ActionResult> ActivityPut(string id, EditActivityDto activityDto)
    {
        activityDto.Id = id;
        return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDto = activityDto }));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "IsActivityHost")]
    public async Task<ActionResult> ActivityDelete(string id)
    {
        return HandleResult(await Mediator.Send(new RemoveActivity.Command { Id = id }));

    }

    [HttpPost("{id}/attend")]
    public async Task<ActionResult> UpdateAttendancePost(string id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
    }

    [HttpPost("/comments")]
    public async Task<ActionResult<GetCommentDto>> CreateCommentPost(CreateComment.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<GetCommentDto>> ActivityCommentsByIdGet(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityCommentsById.Query { ActivityId = id }));
    }
}
