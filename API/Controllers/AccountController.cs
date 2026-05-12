using System;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(SignInManager<User> signInManager) : BaseApiController
{
    public class UserInfo
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public string? DisplayName { get; set; }
        public string? ImageUrl { get; set; }
    }

    [HttpGet("user-info")]
    public async Task<ActionResult<UserInfo>> GetUserInfo()
    {
        var user = await signInManager.UserManager.GetUserAsync(User);

        if (user == null) return Unauthorized();

        return Ok(new UserInfo
        {
            Id = user.Id,
            Email = user.Email!,
            DisplayName = user.DisplayName,
            ImageUrl = user.ImageUrl,
        });
    }


    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }
}
