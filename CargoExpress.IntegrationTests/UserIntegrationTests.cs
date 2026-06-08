using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class UserIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateUser_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = new User("testuser", "hashedpassword", "987654321", false);

        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        var retrievedUser = await userRepository.FindByIdAsync(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal("testuser", retrievedUser.Username);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllUsers_WithMultipleUsers_ShouldReturnAll()
    {
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user1 = new User("user1", "hash1", "111111111", false);
        var user2 = new User("user2", "hash2", "222222222", false);

        await userRepository.AddAsync(user1);
        await userRepository.AddAsync(user2);
        await unitOfWork.CompleteAsync();

        var users = await userRepository.ListAsync();

        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
        Assert.Contains(users, u => u.Username == "user1");
        Assert.Contains(users, u => u.Username == "user2");

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateUser_PasswordHash_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = new User("testuser", "OldHash", "987654321", false);
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        user.UpdatePasswordHash("NewHash456");
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();

        var updatedUser = await userRepository.FindByIdAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("NewHash456", updatedUser.PasswordHash);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);

        var user = await userRepository.FindByIdAsync(999);

        Assert.Null(user);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindUserByUsername_ShouldReturnUser()
    {
        var dbContext = CreateDbContext();
        var userRepository = new UserRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = new User("findme", "hash", "987654321", true);
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        var found = await userRepository.FindByUsernameAsync("findme");

        Assert.NotNull(found);
        Assert.Equal("findme", found.Username);

        CleanupDatabase(dbContext);
    }
}
