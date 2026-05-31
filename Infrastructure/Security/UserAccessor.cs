using System;
using System.Security.Claims;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class UserAccessor(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext) : IUserAccessor
{
    public async Task<User> GetUserAsync()
    {
        return await dbContext.Users.FindAsync(GetUserId()) ?? throw new UnauthorizedAccessException("No user found"); ;
    }

    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("No user found");
    }

    public async Task<User> GetUserWithImagesAsync()
    {
        var userId = GetUserId();

        return await dbContext.Users.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UnauthorizedAccessException("No user found");
    }
}
