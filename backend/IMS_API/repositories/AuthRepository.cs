using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS_API.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _db;

    public AuthRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _db.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public Task<User?> GetByRefreshTokenAsync(int userId, string refreshToken)
        => _db.Users.SingleOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken);

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}