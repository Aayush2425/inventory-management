using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.SalesOrderItemDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesOrderItemsController : ControllerBase
{
    private readonly ISalesOrderItemService _salesOrderItemService;

    public SalesOrderItemsController(ISalesOrderItemService salesOrderItemService)
    {
        _salesOrderItemService = salesOrderItemService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _salesOrderItemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Sales order item not found"));
        }
        return Ok(ApiResponse<SalesOrderItemDto>.SuccessResponse(item, "Sales order item retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var items = await _salesOrderItemService.GetAllAsync();
        return Ok(ApiResponse<List<SalesOrderItemDto>>.SuccessResponse(items, "Sales order items retrieved successfully"));
    }

    [HttpGet("salesorder/{salesOrderId}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetBySalesOrderId(int salesOrderId)
    {
        var items = await _salesOrderItemService.GetBySalesOrderIdAsync(salesOrderId);
        return Ok(ApiResponse<List<SalesOrderItemDto>>.SuccessResponse(items, "Sales order items retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] SalesOrderItemCreateDto request)
    {
        try
        {

            var item = await _salesOrderItemService.CreateAsync(request);
            return Ok(ApiResponse<SalesOrderItemDto>.SuccessResponse(item, "Sales order item created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] SalesOrderItemUpdateDto request)
    {
        try
        {
            var item = await _salesOrderItemService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Sales order item not found"));
            }


            var updatedItem = await _salesOrderItemService.UpdateAsync(id, request);
            if (updatedItem == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Sales order item not found"));
            }
            return Ok(ApiResponse<SalesOrderItemDto>.SuccessResponse(updatedItem, "Sales order item updated successfully"));
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
        var item = await _salesOrderItemService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Sales order item not found"));
        }


        var result = await _salesOrderItemService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Sales order item not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Sales order item deleted successfully"));
    }
}
