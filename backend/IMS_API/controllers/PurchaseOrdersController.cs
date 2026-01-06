using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.PurchaseOrderDTO;
using IMS_API.dtos.PurchaseOrderItemDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly IPurchaseOrderItemService _purchaseOrderItemService;

    public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, IPurchaseOrderItemService purchaseOrderItemService)
    {
        _purchaseOrderService = purchaseOrderService;
        _purchaseOrderItemService = purchaseOrderItemService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _purchaseOrderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Purchase order not found"));
        }
        return Ok(ApiResponse<PurchaseOrderDto>.SuccessResponse(order, "Purchase order retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _purchaseOrderService.GetAllAsync();
        return Ok(ApiResponse<List<PurchaseOrderDto>>.SuccessResponse(orders, "Purchase orders retrieved successfully"));
    }

    [HttpGet("supplier/{supplierId}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetBySupplierId(int supplierId)
    {
        var orders = await _purchaseOrderService.GetBySuppliersIdAsync(supplierId);
        return Ok(ApiResponse<List<PurchaseOrderDto>>.SuccessResponse(orders, "Purchase orders retrieved successfully"));
    }

    [HttpGet("warehouse/{warehouseId}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetByWarehouseId(int warehouseId)
    {
        var orders = await _purchaseOrderService.GetByWarehouseIdAsync(warehouseId);

        Console.WriteLine($"\n\n\n\n {orders[0].Items[0].ProductName}\n\n\n\n");
        return Ok(ApiResponse<List<PurchaseOrderDto>>.SuccessResponse(orders, "Purchase orders retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseWithItemsCreate dto)
    {
        if (dto == null)
            return BadRequest("Invalid request body. DTO is null.");

        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest("At least one purchase order item is required.");

        try
        {
            var order = await _purchaseOrderService.CreateAsync(dto.PurchaseOrder);

            foreach (var item in dto.Items)
            {
                item.PurchaseOrderId = order.Id;
                await _purchaseOrderItemService.CreateAsync(item);
            }

            return Ok(ApiResponse<PurchaseOrderDto>.SuccessResponse(order, "Purchase order created successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] PurchaseWithItemsUpdate request)
    {
        Console.WriteLine($"\n\n\n\nUpdating Purchase Order ID: {request.Items[0].Id}");
        try
        {
            var order = await _purchaseOrderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Purchase order not found"));
            }



            var updatedOrder = await _purchaseOrderService.UpdateAsync(id, request.PurchaseOrder);
            Console.WriteLine($"\n\n\n\nUpdated Purchase Order ID: {updatedOrder.TotalAmount}");
            foreach (var itemDto in request.Items)
            {
                var existingItem = await _purchaseOrderItemService.GetByIdAsync(itemDto.Id);
                if (existingItem != null)
                {
                    // Update existing item
                    await _purchaseOrderItemService.UpdateAsync(itemDto.Id, itemDto);
                }
                else
                {
                    // Create purchaseorderitemcreatedto for new item
                    var newdto = new PurchaseOrderItemCreateDto
                    {
                        ProductId = 0,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        WarehouseId = itemDto.WarehouseId,
                        PurchaseOrderId = id,
                        Status = itemDto.Status
                    };
                    await _purchaseOrderItemService.CreateAsync(newdto);
                }
            }


            if (updatedOrder == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Purchase order not found"));
            }
            return Ok(ApiResponse<PurchaseOrderDto>.SuccessResponse(updatedOrder, "Purchase order updated successfully"));
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
        var order = await _purchaseOrderService.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Purchase order not found"));
        }



        var result = await _purchaseOrderService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Purchase order not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Purchase order deleted successfully"));
    }
}
