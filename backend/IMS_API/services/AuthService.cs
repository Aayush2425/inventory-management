using IMS_API.dtos.AuthDTO;
using IMS_API.Helper;
using IMS_API.Models;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services.Interfaces;

namespace IMS_API.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authrepo;
    private readonly PasswordHasher _passwordhasher;
    private readonly JwtHelper _jwthelper;

    public AuthService(IAuthRepository authRepository, PasswordHasher passwordHasher, JwtHelper jwtHelper)
    {
        _authrepo = authRepository;
        _passwordhasher = passwordHasher;
        _jwthelper = jwtHelper;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _authrepo.GetByUsernameAsync(dto.Username);
        if (user == null)
        {
            throw new InvalidOperationException("Username invalid");
        }
        bool ok = _passwordhasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);
        if (!ok)
        {
            throw new InvalidOperationException("Password Invalid");
        }
        var (token, expires) = _jwthelper.GenerateToken(user);
        var response = new AuthResponseDto
        {
            AccessToken = token,
            AccessTokenExpiry = expires
        };
        return response;
    }
    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        var user = await _authrepo.GetByUsernameAsync(dto.Username);
        if (user != null)
        {
            throw new InvalidOperationException("Username already exist");
        }
        var (hash, salt) = _passwordhasher.HashPassword(dto.Password);
        var newUser = new User
        {
            Username = dto.Username,
            FullName = dto.FullName,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = dto.Role!
        };

        var response = await _authrepo.CreateAsync(newUser);

        return response;
    }

}