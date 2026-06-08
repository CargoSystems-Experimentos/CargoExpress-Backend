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

public class EntrepreneurUnitTest
{
    private readonly Mock<IEntrepreneurRepository> _mockEntrepreneurRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly EntrepreneurCommandService _service;

    private static readonly User ValidUser = new User("entrepreneur@mail.com", "hashed", "987654321", true);

    public EntrepreneurUnitTest()
    {
        _mockEntrepreneurRepo = new Mock<IEntrepreneurRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockEntrepreneurRepo.Setup(r => r.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((Entrepreneur?)null);
        _mockEntrepreneurRepo.Setup(r => r.FindByRucAsync(It.IsAny<string>())).ReturnsAsync((Entrepreneur?)null);
        _mockEntrepreneurRepo.Setup(r => r.AddAsync(It.IsAny<Entrepreneur>())).Returns(Task.CompletedTask);
        _mockUserRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(ValidUser);
        _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        _service = new EntrepreneurCommandService(
            _mockEntrepreneurRepo.Object,
            _mockUserRepo.Object,
            _mockUnitOfWork.Object);
    }

    // --- Entity construction ---

    [Fact]
    public void Create_Entrepreneur_WithParameterlessConstructor_HasDefaultValues()
    {
        var entrepreneur = new Entrepreneur();
        Assert.Equal(0, entrepreneur.Id);
    }

    [Fact]
    public void Create_Entrepreneur_WithAllParameters_SetsPropertiesCorrectly()
    {
        var user = new User("entrepreneur@mail.com", "hashed", "987654321", true);
        var entrepreneur = new Entrepreneur("Empresa Logistica SA", "12345678901", "Av. Lima 123", 1, user);

        Assert.Equal("Empresa Logistica SA", entrepreneur.Name);
        Assert.Equal("12345678901", entrepreneur.Ruc);
        Assert.Equal("Av. Lima 123", entrepreneur.Address);
        Assert.Equal(1, entrepreneur.UserId);
    }

    // --- Repository mock tests ---

    [Fact]
    public async Task GetAll_Entrepreneurs_Success()
    {
        var entrepreneurs = new List<Entrepreneur> { new Entrepreneur(), new Entrepreneur() };
        _mockEntrepreneurRepo.Setup(r => r.ListAsync()).ReturnsAsync(entrepreneurs);

        var result = await _mockEntrepreneurRepo.Object.ListAsync();

        _mockEntrepreneurRepo.Verify(r => r.ListAsync(), Times.Once);
        Assert.Equal(entrepreneurs, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetById_Entrepreneur_ReturnsEntrepreneur_WhenExists()
    {
        var entrepreneur = new Entrepreneur();
        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(entrepreneur);
        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(0)).ReturnsAsync((Entrepreneur?)null);

        var found = await _mockEntrepreneurRepo.Object.FindByIdAsync(1);
        var notFound = await _mockEntrepreneurRepo.Object.FindByIdAsync(0);

        Assert.Equal(entrepreneur, found);
        Assert.Null(notFound);
    }

    [Fact]
    public async Task Add_Entrepreneur_Success()
    {
        var entrepreneur = new Entrepreneur();
        _mockEntrepreneurRepo.Setup(r => r.AddAsync(entrepreneur)).Returns(Task.CompletedTask);

        await _mockEntrepreneurRepo.Object.AddAsync(entrepreneur);

        _mockEntrepreneurRepo.Verify(r => r.AddAsync(entrepreneur), Times.Once);
    }

    // --- Command service: valid creation ---

    [Fact]
    public async Task Create_Entrepreneur_WithValidData_ReturnsEntrepreneur()
    {
        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "12345678901", "Av. Lima 123", 1);

        var result = await _service.Handle(command);

        Assert.NotNull(result);
    }

    // --- Name validation ---

    [Fact]
    public async Task Create_Entrepreneur_WithEmptyName_ThrowsInvalidEntrepreneurNameException()
    {
        var command = new CreateEntrepreneurCommand("", "12345678901", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Entrepreneur_WithNameTooShort_ThrowsInvalidEntrepreneurNameException()
    {
        var command = new CreateEntrepreneurCommand("Empresa", "12345678901", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Entrepreneur_WithNameTooLong_ThrowsInvalidEntrepreneurNameException()
    {
        var longName = new string('A', 61);
        var command = new CreateEntrepreneurCommand(longName, "12345678901", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurNameException>(() => _service.Handle(command));
    }

    // --- RUC validation ---

    [Fact]
    public async Task Create_Entrepreneur_WithEmptyRuc_ThrowsInvalidEntrepreneurRucException()
    {
        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurRucException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Entrepreneur_WithRucNotElevenDigits_ThrowsInvalidEntrepreneurRucException()
    {
        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "123456789", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurRucException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Entrepreneur_WithRucContainingLetters_ThrowsInvalidEntrepreneurRucException()
    {
        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "1234567890A", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurRucException>(() => _service.Handle(command));
    }

    // --- Address validation ---

    [Fact]
    public async Task Create_Entrepreneur_WithEmptyAddress_ThrowsInvalidEntrepreneurAddressException()
    {
        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "12345678901", "", 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurAddressException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Entrepreneur_WithAddressTooLong_ThrowsInvalidEntrepreneurAddressException()
    {
        var longAddress = new string('A', 201);
        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "12345678901", longAddress, 1);

        await Assert.ThrowsAsync<InvalidEntrepreneurAddressException>(() => _service.Handle(command));
    }

    // --- Duplicate checks ---

    [Fact]
    public async Task Create_Entrepreneur_WithDuplicateName_ThrowsDuplicateEntrepreneurNameException()
    {
        _mockEntrepreneurRepo.Setup(r => r.FindByNameAsync("Empresa Logistica SA"))
            .ReturnsAsync(new Entrepreneur());

        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "12345678901", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<DuplicateEntrepreneurNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Entrepreneur_WithDuplicateRuc_ThrowsDuplicateEntrepreneurRucException()
    {
        _mockEntrepreneurRepo.Setup(r => r.FindByRucAsync("12345678901"))
            .ReturnsAsync(new Entrepreneur());

        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "12345678901", "Av. Lima 123", 1);

        await Assert.ThrowsAsync<DuplicateEntrepreneurRucException>(() => _service.Handle(command));
    }

    // --- User not found ---

    [Fact]
    public async Task Create_Entrepreneur_WithNonExistentUserId_ThrowsUserNotFoundException()
    {
        _mockUserRepo.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((User?)null);

        var command = new CreateEntrepreneurCommand("Empresa Logistica SA", "12345678901", "Av. Lima 123", 99);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _service.Handle(command));
    }
}
