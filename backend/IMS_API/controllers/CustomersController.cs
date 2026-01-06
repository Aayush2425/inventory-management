using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.CustomerDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IWarehouseService _warehouseService;

    public CustomersController(ICustomerService customerService, IWarehouseService warehouseService)
    {
        _customerService = customerService;
        _warehouseService = warehouseService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Customer not found"));
        }
        return Ok(ApiResponse<CustomerDto>.SuccessResponse(customer, "Customer retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(ApiResponse<List<CustomerDto>>.SuccessResponse(customers, "Customers retrieved successfully"));
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
        List<CustomerDto[]> customers = new List<CustomerDto[]>();

        foreach (var warehouse in warehouses)
        {
            var warehouseCustomers = await _customerService.GetByWarehouseIdAsync(warehouse.Id);
            customers.Add(warehouseCustomers.ToArray());
        }

        return Ok(ApiResponse<List<CustomerDto[]>>.SuccessResponse(customers, "Customers retrieved successfully"));
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] CustomerCreateDto request)
    {
        try
        {
            var customer = await _customerService.CreateAsync(request);
            return Ok(ApiResponse<CustomerDto>.SuccessResponse(customer, "Customer created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDto request)
    {
        try
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Customer not found"));
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;



            var updatedCustomer = await _customerService.UpdateAsync(id, request);
            if (updatedCustomer == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Customer not found"));
            }
            return Ok(ApiResponse<CustomerDto>.SuccessResponse(updatedCustomer, "Customer updated successfully"));
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
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Customer not found"));
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;



        var result = await _customerService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Customer not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Customer deleted successfully"));
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> Search(
    [FromQuery] string? query = "",
    [FromQuery] string? warehouse = ""
    )
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine($"\n\n\n {warehouse}\n\n\n");
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid or missing token."));

        int userId = int.Parse(userIdClaim);

        var customers = await _customerService.SearchAsync(
            userId,
            query ?? string.Empty,
            warehouse ?? string.Empty
        );

        return Ok(ApiResponse<List<CustomerDto>>.SuccessResponse(customers, "Search results"));
    }

}
