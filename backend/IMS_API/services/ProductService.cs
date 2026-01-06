using IMS_API.dtos.InventoryItemDTO;
using IMS_API.dtos.ProductDTO;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        return MapToResponseDto(product);
    }
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToResponseDto).ToList();
    }
    public async Task<List<ProductDto>> GetByUserIdAsync(int userId)
    {
        var products = await _productRepository.GetByUserIdAsync(userId);
        Console.WriteLine($"\n\n\n\n\n\n Service{products[0].InventoryItems} \n");
        return products.Select(MapToResponseDto).ToList();
    }

    public async Task<List<ProductDto>> GetByCategoryIdAsync(int categoryId)
    {
        var products = _productRepository.GetByCategoryId(categoryId);
        return products.Select(MapToResponseDto).ToList();
    }
    public async Task<ProductDto> CreateAsync(ProductCreateDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            CategoryId = productDto.CategoryId,
            Description = productDto.Description,
            Price = productDto.Price,
            SKU = productDto.SKU,
            IsActive = productDto.IsActive,
            UserId = productDto.UserId,
        };
        var createdProduct = await _productRepository.CreateAsync(product);
        return MapToResponseDto(createdProduct);
    }


    public async Task<ProductDto?> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) throw new InvalidOperationException("Product doesn't exist");
        var productToUpdate = new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            Price = dto.Price,
            SKU = dto.SKU,
            IsActive = dto.IsActive,

            UserId = dto.UserId,

        };

        var updatedProduct = await _productRepository.UpdateAsync(id, productToUpdate);
        return updatedProduct != null ? MapToResponseDto(updatedProduct) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
    public async Task<List<ProductDto>> SearchAsync(int userId, string? query, string? category)
    {
        var products = await _productRepository.SearchAsync(userId, query, category);
        Console.WriteLine($"\n\n\n\n\n\n Service Search {products[0].Category} \n");
        return products.Select(MapToResponseDto).ToList();
    }
    private ProductDto MapToResponseDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            CategoryName = product.Category != null
            ? product.Category.Name
            : null,
            Description = product.Description,
            CreatedAt = product.CreatedAt,
            IsActive = product.IsActive,
            Name = product.Name,
            Price = product.Price,
            SKU = product.SKU,
            UserId = product.UserId,
            InventoryItems = product.InventoryItems.Select(item => new InventoryItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                LastStockUpdateAt = item.LastStockUpdateAt,
                ReorderLevel = item.ReorderLevel,
                WarehouseId = item.WarehouseId
            }).ToList()
        };
    }
}