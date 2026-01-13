using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthenticationModule.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly LoginPermissionContext _context;

    public UserController(LoginPermissionContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        var user = await _context.Users
            .Where(x => x.Id == userId)
            .Select(x => new
            {
                x.Id,
                x.Username,
                x.Email,
                x.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
