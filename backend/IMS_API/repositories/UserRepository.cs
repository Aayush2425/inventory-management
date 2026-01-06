using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetUserByNameAsync(string username)
    {
        return _db.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return _db.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public Task<List<User>> GetAllAsync()
    {
        return _db.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(int id, User user)
    {
        var existingUser = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existingUser == null) return null;

        existingUser.FullName = user.FullName ?? existingUser.FullName;
        existingUser.Role = user.Role ?? existingUser.Role;

        await _db.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user == null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }


}