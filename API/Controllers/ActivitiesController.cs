using Application.Activities.Commands;
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
        return await Mediator.Send(new GetActivityById.Query { Id = id });
    }

    [HttpPost]
    public async Task<ActionResult<string>> ActivityPost(Activity activity)
    {
        return await Mediator.Send(new CreateActivity.Command { Activity = activity });
    }

    [HttpPut]
    public async Task<ActionResult> ActivityPut(Activity activity)
    {
        await Mediator.Send(new EditActivity.Command { Activity = activity });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> ActivityDelete(string id)
    {
        await Mediator.Send(new RemoveActivity.Command { Id = id });

        return Ok();
    }
}
