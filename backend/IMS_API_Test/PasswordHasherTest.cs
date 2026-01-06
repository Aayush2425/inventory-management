using IMS_API.Helper;
namespace IMS_API_Test;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher = new PasswordHasher();
    [Fact]
    public void GeneratePasswordHash_VerifyPassword_SucceedForCorrectPassword()
    {
        // Arrange
        var password = "pass123";
        // Act
        var (hash, salt) = _passwordHasher.HashPassword(password);
        var result = _passwordHasher.VerifyPassword(password, hash, salt);
        // Assert
        Assert.True(result);

    }
    [Fact]
    public void GeneratePasswordHash_VerifyPassword_FailForIncorrectPassword()
    {
        var password = "pass123";
        var wrongPassword = "wrongPass";
        var (hash, salt) = _passwordHasher.HashPassword(password);


        var result = _passwordHasher.VerifyPassword(wrongPassword, hash, salt);

        Assert.False(result);
    }
}
