using Application.Activities.Commands;
using Application.Profiles.Commands;
using Application.Profiles.DTOs;
using Application.Profiles.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpPost("update-image")]
    public async Task<ActionResult<Image>> ImagePost([FromForm] UpdateImage.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDto>> ProfileByIdGet(string id)
    {
        return HandleResult(await Mediator.Send(new GetProfileById.Query { Id = id }));
    }

    [HttpPut]
    public async Task<ActionResult> ProfilePut(UpdateProfile.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("{userId}/follow")]
    public async Task<ActionResult> FollowTogglePost(string userId)
    {
        return HandleResult(await Mediator.Send(new FollowToggle.Command { TargetUserID = userId }));
    }

    [HttpGet("{userId}/follow-list")]
    public async Task<IActionResult> GetFollowingsGet(string userId, GetFollowings.FollowPredicate predicate)
    {
        return HandleResult(await Mediator.Send(new GetFollowings.Query { UserId = userId, Predicate = predicate }));
    }
}
