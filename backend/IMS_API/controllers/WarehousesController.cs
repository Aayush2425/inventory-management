using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.WrehouseDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,owner,manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Warehouse not found"));
        }
        return Ok(ApiResponse<WarehouseResponseDto>.SuccessResponse(warehouse, "Warehouse retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var warehouses = await _warehouseService.GetAllAsync();
        return Ok(ApiResponse<List<WarehouseResponseDto>>.SuccessResponse(warehouses, "Warehouses retrieved successfully"));
    }

    [HttpGet("user/")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetByUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;



        var warehouses = await _warehouseService.GetByUserIdAsync(userIdClaim != null ? int.Parse(userIdClaim) : 0);
        return Ok(ApiResponse<List<WarehouseResponseDto>>.SuccessResponse(warehouses, "User warehouses retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] WarehouseCreateDto request)
    {
        try
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userRole != "Admin" && int.Parse(userIdClaim ?? "0") != request.UserId)
            {
                return Forbid();
            }

            var warehouse = await _warehouseService.CreateAsync(request);
            return Ok(ApiResponse<WarehouseResponseDto>.SuccessResponse(warehouse, "Warehouse creaed successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] WarehouseUpdateDto request)
    {
        try
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Warehouse not found"));
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Allow admin to update any warehouse, owner/manager can update their own
            if (userRole != "Admin" && int.Parse(userIdClaim ?? "0") != warehouse.UserId)
            {
                return Forbid();
            }

            var updatedWarehouse = await _warehouseService.UpdateAsync(id, request);
            if (updatedWarehouse == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Warehouse not found"));
            }
            return Ok(ApiResponse<WarehouseResponseDto>.SuccessResponse(updatedWarehouse, "Warehouse updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Delete(int id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Warehouse not found"));
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userRole != "Admin" && int.Parse(userIdClaim ?? "0") != warehouse.UserId)
        {
            return Forbid();
        }

        var result = await _warehouseService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Warehouse not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Warehouse deleted successfully"));
    }
}
