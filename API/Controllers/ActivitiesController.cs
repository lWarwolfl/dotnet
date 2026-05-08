using Application.Activities.Commands;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> Activities()
    {
        return await Mediator.Send(new GetActivities.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> ActivityById(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityById.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<CreateActivity.Response>> ActivityPost(CreateActivityDTO createActivityDTO)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { CreateActivityDTO = createActivityDTO }));
    }

    [HttpPut]
    public async Task<ActionResult> ActivityPut(Activity activity)
    {
        return HandleResult(await Mediator.Send(new EditActivity.Command { Activity = activity }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> ActivityDelete(string id)
    {
        return HandleResult(await Mediator.Send(new RemoveActivity.Command { Id = id }));

    }
}
