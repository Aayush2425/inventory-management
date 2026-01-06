using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace IMS_API.Helper;

public class PasswordHasher
{
    public (byte[] hash, byte[] salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        var hash = KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 32);

        return (hash, salt);
    }

    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        var hash = KeyDerivation.Pbkdf2(
            password,
            storedSalt,
            KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 32);

        return hash.SequenceEqual(storedHash);
    }
}