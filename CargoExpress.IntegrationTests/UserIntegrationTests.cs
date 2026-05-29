using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class UserIntegrationTests : IntegrationTestBase
{
    /*
    [Fact]
    public async Task CreateUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        
        var user = new User("testuser", "SecurePassword123");

        // Act
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        // Assert
        var retrievedUser = await userRepository.FindByIdAsync(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal("testuser", retrievedUser.Username);
        Assert.Equal("SecurePassword123", retrievedUser.PasswordHash);
        
        // Cleanup
        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllUsers_WithMultipleUsers_ShouldReturnAll()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user1 = new User("user1", "Password123");
        var user2 = new User("user2", "Password456");
        
        await userRepository.AddAsync(user1);
        await userRepository.AddAsync(user2);
        await unitOfWork.CompleteAsync();

        // Act
        var users = await userRepository.ListAsync();

        // Assert
        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
        Assert.Contains(users, u => u.Username == "user1");
        Assert.Contains(users, u => u.Username == "user2");
        
        // Cleanup
        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = new User("testuser", "OldPassword123");
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        // Act - Update password
        user.UpdatePasswordHash("NewPassword456");
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();

        // Assert
        var updatedUser = await userRepository.FindByIdAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("NewPassword456", updatedUser.PasswordHash);
        
        // Cleanup
        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);

        // Act
        var user = await userRepository.FindByIdAsync(999);

        // Assert
        Assert.Null(user);
        
        // Cleanup
        CleanupDatabase(dbContext);
    }
    */
}
