using ACME.CargoExpress.API.IAM.Application.Internal.CommandServices;
using ACME.CargoExpress.API.IAM.Application.Internal.OutboundServices;
using ACME.CargoExpress.API.IAM.Domain.Exceptions;
using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.IAM.Domain.Model.Commands;
using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Services;
using Moq;

namespace CargoExpress.UnitTests;

public class UserUnitTest
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IHashingService> _mockHashingService;
    private readonly Mock<IClientCommandService> _mockClientCommandService;
    private readonly Mock<IEntrepreneurCommandService> _mockEntrepreneurCommandService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UserCommandService _service;

    public UserUnitTest()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _mockHashingService = new Mock<IHashingService>();
        _mockClientCommandService = new Mock<IClientCommandService>();
        _mockEntrepreneurCommandService = new Mock<IEntrepreneurCommandService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUserRepo.Setup(r => r.ExistsByUsername(It.IsAny<string>())).Returns(false);
        _mockUserRepo.Setup(r => r.FindByPhoneAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
        _mockHashingService.Setup(h => h.HashPassword(It.IsAny<string>())).Returns("hashed");
        _mockClientCommandService
            .Setup(s => s.Handle(It.IsAny<CreateClientCommand>()))
            .ReturnsAsync(new Client());
        _mockEntrepreneurCommandService
            .Setup(s => s.Handle(It.IsAny<CreateEntrepreneurCommand>()))
            .ReturnsAsync(new Entrepreneur());

        _service = new UserCommandService(
            _mockUserRepo.Object,
            _mockTokenService.Object,
            _mockHashingService.Object,
            _mockClientCommandService.Object,
            _mockEntrepreneurCommandService.Object,
            _mockUnitOfWork.Object);
    }

    // --- Entity construction ---

    [Fact]
    public void Create_User_WithValidData_SetsPropertiesCorrectly()
    {
        var user = new User("test@email.com", "hashedpwd", "987654321", false);

        Assert.Equal("test@email.com", user.Username);
        Assert.Equal("hashedpwd", user.PasswordHash);
        Assert.Equal("987654321", user.Phone);
        Assert.False(user.Role);
        Assert.True(user.State);
    }

    [Fact]
    public void Create_User_DefaultState_IsTrue()
    {
        var user = new User("admin@mail.com", "pwd", "123456789", true);
        Assert.True(user.State);
    }

    // --- Sign-up: valid scenarios ---

    [Fact]
    public async Task SignUp_AsClient_WithValidData_CompletesSuccessfully()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await _service.Handle(command);
    }

    [Fact]
    public async Task SignUp_AsEntrepreneur_WithValidData_CompletesSuccessfully()
    {
        var command = new SignUpCommand(
            "entrepreneur@mail.com", "Password1!", "987654321", true,
            "Empresa Logistica SA", null, null, "12345678901", "Av. Lima 123");

        await _service.Handle(command);
    }

    // --- Profile validation ---

    [Fact]
    public async Task SignUp_AsClient_WithRuc_ThrowsInvalidProfileException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), "12345678901", null);

        await Assert.ThrowsAsync<InvalidProfileException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_AsClient_WithAddress_ThrowsInvalidProfileException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, "Av. Lima 123");

        await Assert.ThrowsAsync<InvalidProfileException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_AsEntrepreneur_WithDni_ThrowsInvalidProfileException()
    {
        var command = new SignUpCommand(
            "entrepreneur@mail.com", "Password1!", "987654321", true,
            "Empresa Logistica SA", "12345678", null, "12345678901", "Av. Lima 123");

        await Assert.ThrowsAsync<InvalidProfileException>(() => _service.Handle(command));
    }

    // --- Email validation ---

    [Fact]
    public async Task SignUp_WithEmptyEmail_ThrowsInvalidUsernameException()
    {
        var command = new SignUpCommand(
            "", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidUsernameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithInvalidEmailFormat_ThrowsInvalidUsernameException()
    {
        var command = new SignUpCommand(
            "not-an-email", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidUsernameException>(() => _service.Handle(command));
    }

    // --- Password validation ---

    [Fact]
    public async Task SignUp_WithShortPassword_ThrowsInvalidPasswordException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Abc1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidPasswordException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithPasswordNoUppercase_ThrowsInvalidPasswordException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidPasswordException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithPasswordNoDigit_ThrowsInvalidPasswordException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidPasswordException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithPasswordNoSpecialChar_ThrowsInvalidPasswordException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidPasswordException>(() => _service.Handle(command));
    }

    // --- Phone validation ---

    [Fact]
    public async Task SignUp_WithEmptyPhone_ThrowsInvalidUserPhoneException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidUserPhoneException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithPhoneNotNineDigits_ThrowsInvalidUserPhoneException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "12345", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidUserPhoneException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithPhoneContainingLetters_ThrowsInvalidUserPhoneException()
    {
        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "98765432A", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<InvalidUserPhoneException>(() => _service.Handle(command));
    }

    // --- Duplicate checks ---

    [Fact]
    public async Task SignUp_WithDuplicateUsername_ThrowsDuplicateUsernameException()
    {
        _mockUserRepo.Setup(r => r.ExistsByUsername("client@mail.com")).Returns(true);

        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<DuplicateUsernameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task SignUp_WithDuplicatePhone_ThrowsDuplicateUserPhoneException()
    {
        _mockUserRepo.Setup(r => r.FindByPhoneAsync("987654321"))
            .ReturnsAsync(new User("other@mail.com", "hashed", "987654321", false));

        var command = new SignUpCommand(
            "client@mail.com", "Password1!", "987654321", false,
            "Juan Perez Garcia", "12345678", new DateTime(1990, 1, 1), null, null);

        await Assert.ThrowsAsync<DuplicateUserPhoneException>(() => _service.Handle(command));
    }
}
