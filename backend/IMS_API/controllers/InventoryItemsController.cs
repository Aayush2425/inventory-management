using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.InventoryItemDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryItemsController : ControllerBase
{
    private readonly IInventoryItemService _inventoryItemService;

    public InventoryItemsController(IInventoryItemService inventoryItemService)
    {
        _inventoryItemService = inventoryItemService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _inventoryItemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
        }
        return Ok(ApiResponse<InventoryItemDto>.SuccessResponse(item, "Inventory item retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var items = await _inventoryItemService.GetAllAsync();
        return Ok(ApiResponse<List<InventoryItemDto>>.SuccessResponse(items, "Inventory items retrieved successfully"));
    }

    [HttpGet("product/{productId}/warehouse/{warehouseId}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetByProductAndWarehouse(int productId, int warehouseId)
    {
        var item = await _inventoryItemService.GetByProductAndWarehouseAsync(productId, warehouseId);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
        }
        return Ok(ApiResponse<InventoryItemDto>.SuccessResponse(item, "Inventory item retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] InventoryItemCreateDto request)
    {
        try
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;


            var item = await _inventoryItemService.CreateAsync(request);
            return Ok(ApiResponse<InventoryItemDto>.SuccessResponse(item, "Inventory item created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] InventoryItemUpdateDto request)
    {
        try
        {
            var item = await _inventoryItemService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;



            var updatedItem = await _inventoryItemService.UpdateAsync(id, request);
            if (updatedItem == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
            }
            return Ok(ApiResponse<InventoryItemDto>.SuccessResponse(updatedItem, "Inventory item updated successfully"));
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
        var item = await _inventoryItemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;



        var result = await _inventoryItemService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Inventory item deleted successfully"));
    }
}
