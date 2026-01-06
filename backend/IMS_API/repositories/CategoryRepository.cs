using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        return _db.Categories.SingleOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _db.Categories.ToListAsync();
    }


    public async Task<Category> CreateAsync(Category Category)
    {
        _db.Categories.Add(Category);
        await _db.SaveChangesAsync();
        return Category;
    }

    public async Task<Category?> UpdateAsync(int id, Category Category)
    {
        var existingCategory = await _db.Categories.SingleOrDefaultAsync(c => c.Id == id);
        if (existingCategory == null) return null;

        existingCategory.Name = Category.Name ?? existingCategory.Name;
        existingCategory.Description = Category.Description ?? existingCategory.Description;

        await _db.SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var Category = await _db.Categories.SingleOrDefaultAsync(c => c.Id == id);
        if (Category == null) return false;

        _db.Categories.Remove(Category);
        await _db.SaveChangesAsync();
        return true;
    }
}