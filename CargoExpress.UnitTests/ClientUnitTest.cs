using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Application.Internal.CommandServices;
using ACME.CargoExpress.API.User.Domain.Exceptions;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class ClientUnitTest
{
    private readonly Mock<IClientRepository> _mockClientRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ClientCommandService _service;

    private static readonly User ValidUser = new User("client@mail.com", "hashed", "987654321", false);

    public ClientUnitTest()
    {
        _mockClientRepo = new Mock<IClientRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockClientRepo.Setup(r => r.FindByDniAsync(It.IsAny<string>())).ReturnsAsync((Client?)null);
        _mockClientRepo.Setup(r => r.AddAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);
        _mockUserRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(ValidUser);
        _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        _service = new ClientCommandService(
            _mockClientRepo.Object,
            _mockUserRepo.Object,
            _mockUnitOfWork.Object);
    }

    // --- Entity construction ---

    [Fact]
    public void Create_Client_WithParameterlessConstructor_HasDefaultValues()
    {
        var client = new Client();
        Assert.Equal(0, client.Id);
    }

    [Fact]
    public void Create_Client_WithAllParameters_SetsPropertiesCorrectly()
    {
        var user = new User("client@mail.com", "hashed", "987654321", false);
        var client = new Client("Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), 1, user);

        Assert.Equal("Juan Perez Garcia", client.Name);
        Assert.Equal("12345678", client.Dni);
        Assert.Equal(new DateTime(1990, 1, 1), client.BirthDate);
        Assert.Equal(1, client.UserId);
    }

    // --- Repository mock tests ---

    [Fact]
    public async Task GetAll_Clients_Success()
    {
        var clients = new List<Client> { new Client(), new Client() };
        _mockClientRepo.Setup(r => r.ListAsync()).ReturnsAsync(clients);

        var result = await _mockClientRepo.Object.ListAsync();

        _mockClientRepo.Verify(r => r.ListAsync(), Times.Once);
        Assert.Equal(clients, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetById_Client_ReturnsClient_WhenExists()
    {
        var client = new Client();
        _mockClientRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(client);
        _mockClientRepo.Setup(r => r.FindByIdAsync(0)).ReturnsAsync((Client?)null);

        var found = await _mockClientRepo.Object.FindByIdAsync(1);
        var notFound = await _mockClientRepo.Object.FindByIdAsync(0);

        Assert.Equal(client, found);
        Assert.Null(notFound);
    }

    [Fact]
    public async Task Add_Client_Success()
    {
        var client = new Client();
        _mockClientRepo.Setup(r => r.AddAsync(client)).Returns(Task.CompletedTask);

        await _mockClientRepo.Object.AddAsync(client);

        _mockClientRepo.Verify(r => r.AddAsync(client), Times.Once);
    }

    // --- Command service: valid creation ---

    [Fact]
    public async Task Create_Client_WithValidData_ReturnsClient()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), 1);

        var result = await _service.Handle(command);

        Assert.NotNull(result);
    }

    // --- Name validation ---

    [Fact]
    public async Task Create_Client_WithEmptyName_ThrowsInvalidClientNameException()
    {
        var command = new CreateClientCommand("", "12345678", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<InvalidClientNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Client_WithNameTooShort_ThrowsInvalidClientNameException()
    {
        var command = new CreateClientCommand("Juan", "12345678", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<InvalidClientNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Client_WithNameTooLong_ThrowsInvalidClientNameException()
    {
        var longName = new string('A', 61);
        var command = new CreateClientCommand(longName, "12345678", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<InvalidClientNameException>(() => _service.Handle(command));
    }

    // --- DNI validation ---

    [Fact]
    public async Task Create_Client_WithEmptyDni_ThrowsInvalidClientDniException()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<InvalidClientDniException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Client_WithDniNotEightDigits_ThrowsInvalidClientDniException()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "1234567", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<InvalidClientDniException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Client_WithDniContainingLetters_ThrowsInvalidClientDniException()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "1234567A", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<InvalidClientDniException>(() => _service.Handle(command));
    }

    // --- BirthDate validation ---

    [Fact]
    public async Task Create_Client_WithMinValueBirthDate_ThrowsInvalidClientBirthDateException()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "12345678", DateTime.MinValue, 1);

        await Assert.ThrowsAsync<InvalidClientBirthDateException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Client_WithFutureBirthDate_ThrowsInvalidClientBirthDateException()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "12345678", DateTime.Today.AddDays(1), 1);

        await Assert.ThrowsAsync<InvalidClientBirthDateException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Client_WithAgeLessThan18_ThrowsInvalidClientBirthDateException()
    {
        var command = new CreateClientCommand("Juan Perez Garcia", "12345678", DateTime.Today.AddYears(-17), 1);

        await Assert.ThrowsAsync<InvalidClientBirthDateException>(() => _service.Handle(command));
    }

    // --- Duplicate DNI ---

    [Fact]
    public async Task Create_Client_WithDuplicateDni_ThrowsDuplicateClientDniException()
    {
        _mockClientRepo.Setup(r => r.FindByDniAsync("12345678")).ReturnsAsync(new Client());

        var command = new CreateClientCommand("Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), 1);

        await Assert.ThrowsAsync<DuplicateClientDniException>(() => _service.Handle(command));
    }

    // --- User not found ---

    [Fact]
    public async Task Create_Client_WithNonExistentUserId_ThrowsUserNotFoundException()
    {
        _mockUserRepo.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((User?)null);

        var command = new CreateClientCommand("Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), 99);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _service.Handle(command));
    }
}
