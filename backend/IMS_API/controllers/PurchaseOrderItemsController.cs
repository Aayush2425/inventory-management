using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.PurchaseOrderItemDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrderItemsController : ControllerBase
{
    private readonly IPurchaseOrderItemService _purchaseOrderItemService;

    public PurchaseOrderItemsController(IPurchaseOrderItemService purchaseOrderItemService)
    {
        _purchaseOrderItemService = purchaseOrderItemService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _purchaseOrderItemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Purchase order item not found"));
        }
        return Ok(ApiResponse<PurchaseOrderItemDto>.SuccessResponse(item, "Purchase order item retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var items = await _purchaseOrderItemService.GetAllAsync();
        return Ok(ApiResponse<List<PurchaseOrderItemDto>>.SuccessResponse(items, "Purchase order items retrieved successfully"));
    }

    [HttpGet("purchaseorder/{purchaseOrderId}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetByPurchaseOrderId(int purchaseOrderId)
    {
        var items = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(purchaseOrderId);
        return Ok(ApiResponse<List<PurchaseOrderItemDto>>.SuccessResponse(items, "Purchase order items retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderItemCreateDto request)
    {
        try
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;



            var item = await _purchaseOrderItemService.CreateAsync(request);
            return Ok(ApiResponse<PurchaseOrderItemDto>.SuccessResponse(item, "Purchase order item created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] PurchaseOrderItemUpdateDto request)
    {
        try
        {
            var item = await _purchaseOrderItemService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Purchase order item not found"));
            }



            var updatedItem = await _purchaseOrderItemService.UpdateAsync(id, request);
            if (updatedItem == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Purchase order item not found"));
            }
            return Ok(ApiResponse<PurchaseOrderItemDto>.SuccessResponse(updatedItem, "Purchase order item updated successfully"));
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
        var item = await _purchaseOrderItemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Purchase order item not found"));
        }


        var result = await _purchaseOrderItemService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Purchase order item not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Purchase order item deleted successfully"));
    }
}
