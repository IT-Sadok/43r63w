using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Api.Controllers;

[Route("auth/[action]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.RegisterAsync(request, cancellationToken);
        if (!result.Value)
            return BadRequest("Something went wrong");

        return Ok("Succseded");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.LoginAsync(request, cancellationToken);

        if (string.IsNullOrWhiteSpace(result.Value))
            return Unauthorized();

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Result<UserDto?>>> Me()
    {
        var response = await authService.GetMeAsync();
        if(response.IsFailure)
            return Unauthorized();
        return Ok(response.Value);
    }
}
