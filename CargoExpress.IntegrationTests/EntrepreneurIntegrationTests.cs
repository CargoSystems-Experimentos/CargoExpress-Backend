using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class EntrepreneurIntegrationTests : IntegrationTestBase
{
    /*
    [Fact]
    public async Task CreateEntrepreneur_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var entrepreneur = new Entrepreneur
        {
            Name = "Transportes SAC",
            Ruc = "20123456789"
        };

        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();

        var retrieved = await entrepreneurRepository.FindByIdAsync(entrepreneur.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Transportes SAC", retrieved.Name);
        Assert.Equal("20123456789", retrieved.Ruc);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllEntrepreneurs_ShouldReturnMultipleEntrepreneurs()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var entrepreneur1 = new Entrepreneur
        {
            Name = "Empresa Uno SAC",
            Ruc = "20111111111"
        };
        var entrepreneur2 = new Entrepreneur
        {
            Name = "Empresa Dos SAC",
            Ruc = "20222222222"
        };

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

        var entrepreneur = new Entrepreneur
        {
            Name = "Old Company",
            Ruc = "20999999999"
        };

        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();

        entrepreneur.Update(new UpdateEntrepreneurCommand(
            entrepreneur.Id, "New Company SAC", "20888888888", entrepreneur.UserId));
        entrepreneurRepository.Update(entrepreneur);
        await unitOfWork.CompleteAsync();

        var updated = await entrepreneurRepository.FindByIdAsync(entrepreneur.Id);
        Assert.NotNull(updated);
        Assert.Equal("New Company SAC", updated.Name);
        Assert.Equal("20888888888", updated.Ruc);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindEntrepreneurByUserId_ShouldReturnEntrepreneur()
    {
        var dbContext = CreateDbContext();
        var entrepreneurRepository = new EntrepreneurRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var entrepreneur = new Entrepreneur
        {
            Name = "Cargo Logistics SAC",
            Ruc = "20567812345"
        };

        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();

        var found = await entrepreneurRepository.FindByUserIdAsync(entrepreneur.UserId);

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
    */
}
