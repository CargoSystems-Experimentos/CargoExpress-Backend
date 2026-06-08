using ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;
using ACME.CargoExpress.API.Registration.Domain.Exceptions;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class DriverUnitTest
{
    private readonly Mock<IDriverRepository> _mockDriverRepo;
    private readonly Mock<IEntrepreneurRepository> _mockEntrepreneurRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DriverCommandService _service;

    private static readonly Entrepreneur ValidEntrepreneur = new Entrepreneur();

    public DriverUnitTest()
    {
        _mockDriverRepo = new Mock<IDriverRepository>();
        _mockEntrepreneurRepo = new Mock<IEntrepreneurRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(ValidEntrepreneur);
        _mockDriverRepo.Setup(r => r.FindByNameAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Driver?)null);
        _mockDriverRepo.Setup(r => r.FindByDniAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Driver?)null);
        _mockDriverRepo.Setup(r => r.AddAsync(It.IsAny<Driver>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        _service = new DriverCommandService(
            _mockDriverRepo.Object,
            _mockEntrepreneurRepo.Object,
            _mockUnitOfWork.Object);
    }

    // --- Entity construction ---

    [Fact]
    public void Create_Driver_WithParameterlessConstructor_DefaultStateIsAvailable()
    {
        var driver = new Driver();
        Assert.Equal("AVAILABLE", driver.State);
    }

    [Fact]
    public void Create_Driver_WithAllParameters_SetsPropertiesCorrectly()
    {
        var entrepreneur = new Entrepreneur();
        var driver = new Driver("Juan Perez Garcia", "12345678", "A-IIb", "987654321", 1, entrepreneur);

        Assert.Equal("Juan Perez Garcia", driver.Name);
        Assert.Equal("12345678", driver.Dni);
        Assert.Equal("A-IIb", driver.License);
        Assert.Equal("987654321", driver.ContactNumber);
        Assert.Equal(1, driver.EntrepreneurId);
        Assert.Equal("AVAILABLE", driver.State);
    }

    // --- Repository mock tests ---

    [Fact]
    public async Task GetAll_Drivers_Success()
    {
        var drivers = new List<Driver> { new Driver(), new Driver() };
        _mockDriverRepo.Setup(r => r.ListAsync()).ReturnsAsync(drivers);

        var result = await _mockDriverRepo.Object.ListAsync();

        _mockDriverRepo.Verify(r => r.ListAsync(), Times.Once);
        Assert.Equal(drivers, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetById_Driver_ReturnsDriver_WhenExists()
    {
        var driver = new Driver();
        _mockDriverRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(driver);
        _mockDriverRepo.Setup(r => r.FindByIdAsync(0)).ReturnsAsync((Driver?)null);

        var found = await _mockDriverRepo.Object.FindByIdAsync(1);
        var notFound = await _mockDriverRepo.Object.FindByIdAsync(0);

        Assert.Equal(driver, found);
        Assert.Null(notFound);
    }

    [Fact]
    public async Task Add_Driver_Success()
    {
        var driver = new Driver();
        _mockDriverRepo.Setup(r => r.AddAsync(driver)).Returns(Task.CompletedTask);

        await _mockDriverRepo.Object.AddAsync(driver);

        _mockDriverRepo.Verify(r => r.AddAsync(driver), Times.Once);
    }

    // --- Command service: valid creation ---

    [Fact]
    public async Task Create_Driver_WithValidData_ReturnsDriver()
    {
        var command = new CreateDriverCommand("Juan Perez Garcia", "12345678", "A-IIb", "987654321", 1);

        var result = await _service.Handle(command);

        Assert.NotNull(result);
    }

    // --- Name validation ---

    [Fact]
    public async Task Create_Driver_WithEmptyName_ThrowsInvalidDriverNameException()
    {
        var command = new CreateDriverCommand("", "12345678", "A-IIb", "987654321", 1);

        await Assert.ThrowsAsync<InvalidDriverNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Driver_WithNameTooLong_ThrowsDriverNameTooLongException()
    {
        var longName = new string('A', 61);
        var command = new CreateDriverCommand(longName, "12345678", "A-IIb", "987654321", 1);

        await Assert.ThrowsAsync<DriverNameTooLongException>(() => _service.Handle(command));
    }

    // --- DNI validation ---

    [Fact]
    public async Task Create_Driver_WithDniContainingLetters_ThrowsInvalidDriverDniFormatException()
    {
        var command = new CreateDriverCommand("Juan Perez Garcia", "1234567A", "A-IIb", "987654321", 1);

        await Assert.ThrowsAsync<InvalidDriverDniFormatException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Driver_WithDniNotEightDigits_ThrowsInvalidDriverDniLengthException()
    {
        var command = new CreateDriverCommand("Juan Perez Garcia", "1234567", "A-IIb", "987654321", 1);

        await Assert.ThrowsAsync<InvalidDriverDniLengthException>(() => _service.Handle(command));
    }

    // --- License validation ---

    [Fact]
    public async Task Create_Driver_WithEmptyLicense_ThrowsInvalidDriverLicenseException()
    {
        var command = new CreateDriverCommand("Juan Perez Garcia", "12345678", "", "987654321", 1);

        await Assert.ThrowsAsync<InvalidDriverLicenseException>(() => _service.Handle(command));
    }

    // --- ContactNumber validation ---

    [Fact]
    public async Task Create_Driver_WithContactNumberContainingLetters_ThrowsInvalidDriverPhoneFormatException()
    {
        var command = new CreateDriverCommand("Juan Perez Garcia", "12345678", "A-IIb", "98765432A", 1);

        await Assert.ThrowsAsync<InvalidDriverPhoneFormatException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Driver_WithContactNumberNotNineDigits_ThrowsInvalidDriverPhoneLengthException()
    {
        var command = new CreateDriverCommand("Juan Perez Garcia", "12345678", "A-IIb", "12345", 1);

        await Assert.ThrowsAsync<InvalidDriverPhoneLengthException>(() => _service.Handle(command));
    }

    // --- Entrepreneur not found ---

    [Fact]
    public async Task Create_Driver_WithNonExistentEntrepreneurId_ThrowsArgumentException()
    {
        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((Entrepreneur?)null);

        var command = new CreateDriverCommand("Juan Perez Garcia", "12345678", "A-IIb", "987654321", 99);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("empresario", ex.Message);
    }

    // --- Duplicate name ---

    [Fact]
    public async Task Create_Driver_WithDuplicateName_ThrowsDuplicateDriverNameException()
    {
        _mockDriverRepo.Setup(r => r.FindByNameAsync("Juan Perez Garcia", It.IsAny<int>()))
            .ReturnsAsync(new Driver());

        var command = new CreateDriverCommand("Juan Perez Garcia", "12345678", "A-IIb", "987654321", 1);

        await Assert.ThrowsAsync<DuplicateDriverNameException>(() => _service.Handle(command));
    }

    // --- UpdateDriverStateCommand validation ---

    [Fact]
    public async Task Update_DriverState_WithValidState_ReturnsDriver()
    {
        var driver = new Driver();
        _mockDriverRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(driver);

        var command = new UpdateDriverStateCommand(1, "UNAVAILABLE");
        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("UNAVAILABLE", result!.State);
    }

    [Fact]
    public async Task Update_DriverState_WithInvalidState_ThrowsInvalidDriverStateException()
    {
        var driver = new Driver();
        _mockDriverRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(driver);

        var command = new UpdateDriverStateCommand(1, "INVALID_STATE");

        await Assert.ThrowsAsync<InvalidDriverStateException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Update_DriverState_WithAvailableState_Succeeds()
    {
        var driver = new Driver();
        _mockDriverRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(driver);

        var command = new UpdateDriverStateCommand(1, "AVAILABLE");
        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("AVAILABLE", result!.State);
    }

    [Fact]
    public async Task Update_DriverState_WithInactiveState_Succeeds()
    {
        var driver = new Driver();
        _mockDriverRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(driver);

        var command = new UpdateDriverStateCommand(1, "INACTIVE");
        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("INACTIVE", result!.State);
    }
}
