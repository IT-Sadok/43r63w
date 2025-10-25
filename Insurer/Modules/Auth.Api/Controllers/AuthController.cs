using Auth.Api.Dtos;
using Auth.Api.Services;
using Auth.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Api.Controllers;

[Route("auth/[action]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.RegisterAsync(request);
        if (!result)
            return BadRequest("Something went wrong");

        return Ok("Succseded");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.LoginAsync(request);

        if (string.IsNullOrWhiteSpace(result))
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
            return Unauthorized();

        var response = await authService.GetMeAsync(userId!);

        return response;
    }
}
