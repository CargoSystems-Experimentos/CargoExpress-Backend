using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class EntrepreneurIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateEntrepreneur_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = new Entrepreneur("Transportes SAC", "20123456789", "Av. Principal 100", user.Id, user);

        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();

        var retrieved = await entrepreneurRepository.FindByIdAsync(entrepreneur.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Transportes SAC", retrieved.Name);
        Assert.Equal("20123456789", retrieved.Ruc);
        Assert.Equal("Av. Principal 100", retrieved.Address);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllEntrepreneurs_ShouldReturnMultipleEntrepreneurs()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user1 = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var user2 = await CreateUserAsync(dbContext, "entrepreneur2", "900000002", true);

        var entrepreneur1 = new Entrepreneur("Empresa Uno SAC", "20111111111", "Av. Uno 1", user1.Id, user1);
        var entrepreneur2 = new Entrepreneur("Empresa Dos SAC", "20222222222", "Av. Dos 2", user2.Id, user2);

        await entrepreneurRepository.AddAsync(entrepreneur1);
        await entrepreneurRepository.AddAsync(entrepreneur2);
        await unitOfWork.CompleteAsync();

        var entrepreneurs = await entrepreneurRepository.ListAsync();

        Assert.NotNull(entrepreneurs);
        Assert.Equal(2, entrepreneurs.Count());
        Assert.Contains(entrepreneurs, e => e.Name == "Empresa Uno SAC");
        Assert.Contains(entrepreneurs, e => e.Name == "Empresa Dos SAC");

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateEntrepreneur_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = new Entrepreneur("Old Company SAC", "20999999999", "Old Address 789", user.Id, user);

        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();

        entrepreneur.Update(new UpdateEntrepreneurCommand(
            entrepreneur.Id, "New Company SAC", "20888888888", "New Address 456", entrepreneur.UserId));
        entrepreneurRepository.Update(entrepreneur);
        await unitOfWork.CompleteAsync();

        var updated = await entrepreneurRepository.FindByIdAsync(entrepreneur.Id);
        Assert.NotNull(updated);
        Assert.Equal("New Company SAC", updated.Name);
        Assert.Equal("20888888888", updated.Ruc);
        Assert.Equal("New Address 456", updated.Address);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindEntrepreneurByUserId_ShouldReturnEntrepreneur()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = new Entrepreneur("Cargo Logistics SAC", "20567812345", "Av. Logistica 500", user.Id, user);

        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();

        var found = await entrepreneurRepository.FindByUserIdAsync(user.Id);

        Assert.NotNull(found);
        Assert.Equal("Cargo Logistics SAC", found.Name);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetEntrepreneurById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(999);

        Assert.Null(entrepreneur);

        CleanupDatabase(dbContext);
    }
}
