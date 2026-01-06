using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.SupplierDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly IWarehouseService _warehouseService;

    public SuppliersController(ISupplierService supplierService, IWarehouseService warehouseService)
    {
        _supplierService = supplierService;
        _warehouseService = warehouseService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Supplier not found"));
        }
        return Ok(ApiResponse<SupplierDto>.SuccessResponse(supplier, "Supplier retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(ApiResponse<List<SupplierDto>>.SuccessResponse(suppliers, "Suppliers retrieved successfully"));
    }
    [HttpGet("warehouse")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetByWarehouseId()
    {
        Console.WriteLine("\n\n\n\n\nEntered GetByWarehouseId");
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("\n\n\n\n\nUser ID Claim: " + userIdClaim);
        var warehouses = await _warehouseService.GetByUserIdAsync(int.Parse(userIdClaim ?? "0"));

        if (warehouses.Count == 0)
        {
            throw new InvalidOperationException("No warehouse assigned to the user.");
        }

        Console.WriteLine("\n\n\n\n\nNo of warehouses: \n" + warehouses[0].Id);
        List<SupplierDto[]> suppliers = new List<SupplierDto[]>();

        foreach (var warehouse in warehouses)
        {
            var warehouseSuppliers = await _supplierService.GetByWarehouseIdAsync(warehouse.Id);
            suppliers.Add(warehouseSuppliers.ToArray());
        }

        return Ok(ApiResponse<List<SupplierDto[]>>.SuccessResponse(suppliers, "Suppliers retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] SupplierCreateDto request)
    {
        try
        {


            var supplier = await _supplierService.CreateAsync(request);
            return Ok(ApiResponse<SupplierDto>.SuccessResponse(supplier, "Supplier created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateDto request)
    {
        try
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Supplier not found"));
            }


            var updatedSupplier = await _supplierService.UpdateAsync(id, request);
            if (updatedSupplier == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Supplier not found"));
            }
            return Ok(ApiResponse<SupplierDto>.SuccessResponse(updatedSupplier, "Supplier updated successfully"));
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
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Supplier not found"));
        }


        var result = await _supplierService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Supplier not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Supplier deleted successfully"));
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> Search(
    [FromQuery] string? query = "",
    [FromQuery] string? warehouse = ""
    )
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid or missing token."));

        int userId = int.Parse(userIdClaim);

        var suppliers = await _supplierService.SearchAsync(
            userId,
            query ?? string.Empty,
            warehouse ?? string.Empty
        );

        return Ok(ApiResponse<List<SupplierDto>>.SuccessResponse(suppliers, "Search results"));
    }

}
