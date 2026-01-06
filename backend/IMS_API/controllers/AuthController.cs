using System.Security.Claims;
using IMS_API.dtos.AuthDTO;
using IMS_API.Models;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authservice;

    public AuthController(IAuthService authService)
    {
        _authservice = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        try
        {
            var response = await _authservice.RegisterAsync(request);
            if (response == null)
            {
                throw new InvalidOperationException("something went wrong");
            }
            return Ok(ApiResponse<User>.SuccessResponse(response, "user registered successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            var response = await _authservice.LoginAsync(request);
            if (response == null)
            {
                throw new InvalidOperationException("something went wrong");
            }
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response, "user logged in successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst("UserID")?.Value;

        return Ok(new
        {
            UserId = User.FindFirst("UserID")?.Value,
            Username = User.Identity?.Name,
            Role = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}