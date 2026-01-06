using IMS_API.dtos.AuthDTO;
using IMS_API.dtos.CategoryDTO;
using IMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _CategoryService;

    public CategoryController(ICategoryService CategoryService)
    {
        _CategoryService = CategoryService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var Category = await _CategoryService.GetByIdAsync(id);
        if (Category == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
        }
        return Ok(ApiResponse<CategoryDto>.SuccessResponse(Category, "Category retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Owner,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var Category = await _CategoryService.GetAllAsync();
        return Ok(ApiResponse<List<CategoryDto>>.SuccessResponse(Category, "Categorys retrieved successfully"));
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto request)
    {
        try
        {


            var Category = await _CategoryService.CreateAsync(request);
            return Ok(ApiResponse<CategoryDto>.SuccessResponse(Category, "Category creaed successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto request)
    {
        try
        {
            var Category = await _CategoryService.GetByIdAsync(id);
            if (Category == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
            }


            var updatedCategory = await _CategoryService.UpdateAsync(id, request);
            if (updatedCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
            }
            return Ok(ApiResponse<CategoryDto>.SuccessResponse(updatedCategory, "Category updated successfully"));
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
        var Category = await _CategoryService.GetByIdAsync(id);
        if (Category == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
        }



        var result = await _CategoryService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
        }
        return Ok(ApiResponse<object>.SuccessResponse(null, "Category deleted successfully"));
    }
}
