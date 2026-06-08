using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class ClientIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateClient_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "client1", "900000001", false);
        var client = new Client("Juan Gomez", "12345678", new DateTime(1990, 5, 15), user.Id, user);

        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();

        var retrieved = await clientRepository.FindByIdAsync(client.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Juan Gomez", retrieved.Name);
        Assert.Equal("12345678", retrieved.Dni);
        Assert.Equal(new DateTime(1990, 5, 15), retrieved.BirthDate);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllClients_ShouldReturnMultipleClients()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user1 = await CreateUserAsync(dbContext, "client1", "900000001", false);
        var user2 = await CreateUserAsync(dbContext, "client2", "900000002", false);

        var client1 = new Client("Client One", "11111111", new DateTime(1985, 3, 10), user1.Id, user1);
        var client2 = new Client("Client Two", "22222222", new DateTime(1995, 7, 20), user2.Id, user2);

        await clientRepository.AddAsync(client1);
        await clientRepository.AddAsync(client2);
        await unitOfWork.CompleteAsync();

        var clients = await clientRepository.ListAsync();

        Assert.NotNull(clients);
        Assert.Equal(2, clients.Count());
        Assert.Contains(clients, c => c.Name == "Client One");
        Assert.Contains(clients, c => c.Name == "Client Two");

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateClient_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "client1", "900000001", false);
        var client = new Client("Original Name", "99999999", new DateTime(1990, 1, 1), user.Id, user);

        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();

        client.Update(new UpdateClientCommand(client.Id, "Updated Name", "88888888", new DateTime(1991, 6, 15), client.UserId));
        clientRepository.Update(client);
        await unitOfWork.CompleteAsync();

        var updated = await clientRepository.FindByIdAsync(client.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal("88888888", updated.Dni);
        Assert.Equal(new DateTime(1991, 6, 15), updated.BirthDate);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindClientByUserId_ShouldReturnClient()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "client1", "900000001", false);
        var client = new Client("Maria Lopez", "87654321", new DateTime(1988, 9, 25), user.Id, user);

        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();

        var found = await clientRepository.FindByUserIdAsync(user.Id);

        Assert.NotNull(found);
        Assert.Equal("Maria Lopez", found.Name);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetClientById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);

        var client = await clientRepository.FindByIdAsync(999);

        Assert.Null(client);

        CleanupDatabase(dbContext);
    }
}
