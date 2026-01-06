using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.ProductDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IInventoryItemService _inventoryItemService;

    public ProductController(IProductService productService, ICategoryService categoryService, IInventoryItemService inventoryItemService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _inventoryItemService = inventoryItemService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Product not found"));
        }
        return Ok(ApiResponse<ProductDto>.SuccessResponse(product, "Product retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(ApiResponse<List<ProductDto>>.SuccessResponse(products, "Products retrieved successfully"));
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userRole != "Admin" && int.Parse(userIdClaim ?? "0") != userId)
        {
            return Forbid();
        }

        var products = await _productService.GetByUserIdAsync(userId);
        Console.WriteLine($"\n\n\n\n\n\n Contoller{products[0].CategoryName} \n");
        return Ok(ApiResponse<List<ProductDto>>.SuccessResponse(products, "User Products retrieved successfully"));
    }
    [HttpGet("category/{categoryId}")]
    [Authorize(Roles = "Owner,Manager")]

    public async Task<IActionResult> GetByCategoryID(int categoryId)
    {
        Console.WriteLine($"\n\n\n\n\n\n Contoller \n");
        var catrgory = await _categoryService.GetByIdAsync(categoryId);
        if (catrgory == null) throw new InvalidOperationException("Category doesn't exist");

        var products = await _productService.GetByCategoryIdAsync(categoryId);

        return Ok(ApiResponse<List<ProductDto>>.SuccessResponse(products, "Category wise Products retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> Create([FromBody] ProductWithInventoryCreateDto request)
    {
        Console.WriteLine($"\n\n\n\n\n{request}");
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            request.Product.UserId = int.Parse(userIdClaim ?? "0");

            var product = await _productService.CreateAsync(request.Product);
            request.Inventory.ProductId = product.Id;
            var inventroy = await _inventoryItemService.CreateAsync(request.Inventory);


            return Ok(ApiResponse<ProductWithInventoryCreateDto>.SuccessResponse(request, "Product created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductWithInventoryUpdateDto request)
    {
        Console.WriteLine($"\n\n\n\n\n outside try :{request}\n");

        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            request.Product.UserId = int.Parse(userIdClaim ?? "0");
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("product not found"));
            }


            var updatedProduct = await _productService.UpdateAsync(id, request.Product);
            if (updatedProduct == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Product not found"));
            }
            Console.WriteLine($"\n\n\n\n\n After product update in controller{updatedProduct.Price}\n");
            var updatedInventory = await _inventoryItemService.UpdateByProductIdAsync(id, request.Inventory);
            if (updatedInventory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Inventory item not found"));
            }


            return Ok(ApiResponse<ProductWithInventoryUpdateDto>.SuccessResponse(request, "Product updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("product not found"));
        }



        var result = await _productService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("product not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "product deleted successfully"));
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> Search(
    [FromQuery] string? query = "",
    [FromQuery] string? category = ""
    )
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid or missing token."));

        int userId = int.Parse(userIdClaim);

        var products = await _productService.SearchAsync(
            userId,
            query ?? string.Empty,
            category ?? string.Empty
        );

        Console.WriteLine($"\n\n\n\n\n\n Controller Search {products[0].CategoryName} \n");

        return Ok(ApiResponse<List<ProductDto>>.SuccessResponse(products, "Search results"));
    }


}
