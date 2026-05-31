using Application.Profiles.Commands;
using Application.Profiles.DTOs;
using Application.Profiles.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpPost("update-image")]
    public async Task<ActionResult<List<Image>>> ImagePost([FromForm] UpdateImage.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDto>> ProfileByIdGet(string id)
    {
        return HandleResult(await Mediator.Send(new GetProfileById.Query { Id = id }));
    }
}
