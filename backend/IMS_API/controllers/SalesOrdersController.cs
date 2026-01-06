using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.SalesOrderDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesOrdersController : ControllerBase
{
    private readonly ISalesOrderService _salesOrderService;
    private readonly ISalesOrderItemService _salesOrderItemService;

    public SalesOrdersController(ISalesOrderService salesOrderService, ISalesOrderItemService salesOrderItemService)
    {
        _salesOrderService = salesOrderService;
        _salesOrderItemService = salesOrderItemService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _salesOrderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Sales order not found"));
        }
        return Ok(ApiResponse<SalesOrderDto>.SuccessResponse(order, "Sales order retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _salesOrderService.GetAllAsync();
        return Ok(ApiResponse<List<SalesOrderDto>>.SuccessResponse(orders, "Sales orders retrieved successfully"));
    }

    [HttpGet("customer/{customerId}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetByCustomerId(int customerId)
    {
        var orders = await _salesOrderService.GetByCustomerIdAsync(customerId);
        return Ok(ApiResponse<List<SalesOrderDto>>.SuccessResponse(orders, "Sales orders retrieved successfully"));
    }



    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] SalesWithItemsCreate dto)
    {
        if (dto == null)
            return BadRequest("Invalid request body. DTO is null.");

        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest("At least one sales order item is required.");

        try
        {
            var order = await _salesOrderService.CreateAsync(dto.SalesOrder);

            foreach (var item in dto.Items)
            {
                item.SalesOrderId = order.Id;
                await _salesOrderItemService.CreateAsync(item);
            }

            var createdOrder = await _salesOrderService.GetByIdAsync(order.Id);
            return Ok(ApiResponse<SalesOrderDto>.SuccessResponse(createdOrder, "Sales order created successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] SalesWithItemsUpdate request)
    {
        try
        {
            var order = await _salesOrderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Sales order not found"));
            }

            var updatedOrder = await _salesOrderService.UpdateAsync(id, request.SalesOrder);
            if (updatedOrder == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Sales order not found"));
            }

            // Delete existing items and create new ones
            var existingItems = order.Items;
            foreach (var existingItem in existingItems)
            {
                await _salesOrderItemService.DeleteAsync(existingItem.Id);
            }

            foreach (var item in request.Items)
            {
                item.SalesOrderId = id;
                await _salesOrderItemService.CreateAsync(item);
            }

            var finalOrder = await _salesOrderService.GetByIdAsync(id);
            return Ok(ApiResponse<SalesOrderDto>.SuccessResponse(finalOrder, "Sales order updated successfully"));
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
        var order = await _salesOrderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Sales order not found"));
        }

        var result = await _salesOrderService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Sales order not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Sales order deleted successfully"));
    }
}

