using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class ClientIntegrationTests : IntegrationTestBase
{
    /*
    [Fact]
    public async Task CreateClient_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var client = new Client
        {
            Name = "Juan Gomez",
            Dni = "12345678"
        };

        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();

        var retrieved = await clientRepository.FindByIdAsync(client.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Juan Gomez", retrieved.Name);
        Assert.Equal("12345678", retrieved.Dni);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllClients_ShouldReturnMultipleClients()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var client1 = new Client { Name = "Client One", Dni = "11111111" };
        var client2 = new Client { Name = "Client Two", Dni = "22222222" };

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

        var client = new Client { Name = "Original Name", Dni = "99999999" };

        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();

        client.Update(new UpdateClientCommand(client.Id, "Updated Name", "88888888", client.UserId));
        clientRepository.Update(client);
        await unitOfWork.CompleteAsync();

        var updated = await clientRepository.FindByIdAsync(client.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal("88888888", updated.Dni);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindClientByUserId_ShouldReturnClient()
    {
        var dbContext = CreateDbContext();
        var clientRepository = new ClientRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var client = new Client { Name = "Maria Lopez", Dni = "87654321" };

        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();

        var found = await clientRepository.FindByUserIdAsync(client.UserId);

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
        */
}
