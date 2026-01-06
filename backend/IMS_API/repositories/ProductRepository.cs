using System.Data;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    public ProductRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return _db.Products.SingleOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Product>> GetAllAsync()
    {
        return _db.Products.ToListAsync();
    }
    public Task<List<Product>> GetByUserIdAsync(int userId)
    {
        return _db.Products.Include(p => p.Category).Include(p => p.InventoryItems).Where(p => p.UserId == userId).ToListAsync();
    }
    public List<Product> GetByCategoryId(int categoryId)
    {
        var products = new List<Product>();

        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        using (var command = new SqlCommand("Product_Get_By_Category", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            // Add missing parameter
            command.Parameters.AddWithValue("@id", categoryId);

            connection.Open();

            using (var adapter = new SqlDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);

                products = table.AsEnumerable()
                    .Select(row => new Product
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name")!,
                        Description = row.Field<string>("Description"),
                        SKU = row.Field<string>("SKU"),
                        Price = row.Field<decimal>("Price"),
                        IsActive = row.Field<bool>("IsActive"),
                        CategoryId = row.Field<int>("CategoryId"),
                        Category = new Category
                        {
                            Id = row.Field<int>("CategoryId"),
                            Name = row.Field<string>("CategoryName")!,
                            Description = row.Field<string>("CategoryDescription")
                        },
                        UserId = row.Field<int>("UserId"),
                        CreatedAt = row.Field<DateTime>("CreatedAt"),
                    })
                    .ToList();
            }
        }
        return products;
    }



    public async Task<Product> CreateAsync(Product product)
    {
        Console.WriteLine($"\n\n\n\n\n${product}\n\n");
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }
    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existingProduct = await _db.Products.SingleOrDefaultAsync(w => w.Id == id);
        if (existingProduct == null) return null;

        existingProduct.Name = product.Name ?? existingProduct.Name;
        existingProduct.CategoryId = product.CategoryId ?? existingProduct.CategoryId;
        existingProduct.UserId = product.UserId ?? existingProduct.UserId;
        existingProduct.Description = product.Description ?? existingProduct.Description;
        existingProduct.Price = product.Price;
        existingProduct.SKU = product.SKU ?? existingProduct.SKU;
        existingProduct.IsActive = product.IsActive;



        await _db.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var Product = await _db.Products.SingleOrDefaultAsync(w => w.Id == id);
        if (Product == null) return false;

        _db.Products.Remove(Product);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<List<Product>> SearchAsync(int userId, string? query, string? category)
    {
        var products = new List<Product>();

        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        using (var command = new SqlCommand("Product_Search_By_Name_And_Category", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Query", query);
            command.Parameters.AddWithValue("@Category", category);

            connection.Open();

            using (var adapter = new SqlDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);

                products = table.AsEnumerable()
                    .Select(row => new Product
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name")!,
                        Description = row.Field<string>("Description"),
                        SKU = row.Field<string>("SKU"),
                        Price = row.Field<decimal>("Price"),
                        IsActive = row.Field<bool>("IsActive"),
                        CategoryId = row.Field<int>("CategoryId"),
                        Category = new Category
                        {
                            Id = row.Field<int>("Id"),
                            Name = row.Field<string>("CategoryName")!,
                            Description = row.Field<string>("Description")
                        },
                        UserId = row.Field<int>("UserId"),
                        CreatedAt = row.Field<DateTime>("CreatedAt"),
                        InventoryItems = row.Field<int>("InventoryItemCount") > 0 ? new List<InventoryItem>
                        {
                            new InventoryItem
                            {
                                Id = row.Field<int>("InventoryItemId"),
                                ProductId = row.Field<int>("Id"),
                                Quantity = row.Field<int>("Quantity"),
                                ReorderLevel = row.Field<int>("ReorderLevel"),
                                WarehouseId = row.Field<int>("WarehouseId"),
                                ReorderQuantity = row.Field<int>("ReorderQuantity"),
                                CreatedAt = row.Field<DateTime>("InventoryCreatedAt"),
                            }
                        } : new List<InventoryItem>()
                    })
                    .ToList();
            }
            connection.Close();
        }

        return products;
    }
}
