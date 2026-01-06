using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.UserDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("User not found"));
        }
        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(user, "User retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(users, "Users retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] UserCreateDto request)
    {
        try
        {
            var user = await _userService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id },
                ApiResponse<UserResponseDto>.SuccessResponse(user, "User created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto request)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, request);
            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("User not found"));
            }
            return Ok(ApiResponse<UserResponseDto>.SuccessResponse(user, "User updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("User not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "User deleted successfully"));
    }
}
