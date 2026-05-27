using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;
public class AlertUnitTest
{
    [Fact]
    public async Task GetAll_Alerts_Success()
    {
        // Arrange
        var alerts = new List<Alert> { new Alert(), new Alert() };
        var mockAlertRepository = new Mock<IAlertRepository>();
        mockAlertRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(alerts);

        // Act
        var returnedAlerts = await mockAlertRepository.Object.ListAsync();

        // Assert
        mockAlertRepository.Verify(repo => repo.ListAsync(), Times.Once);
        Assert.Equal(alerts, returnedAlerts);
        Assert.Equal(alerts.Count, returnedAlerts.Count());
    }

    [Fact]
    public async Task GetById_Alert_Success()
    {
        // Arrange
        int validId = 1;
        var alert = new Alert();
        var mockAlertRepository = new Mock<IAlertRepository>();
        mockAlertRepository.Setup(repo => repo.FindByIdAsync(validId)).ReturnsAsync(alert);

        // Act
        var returnedAlert = await mockAlertRepository.Object.FindByIdAsync(validId);

        // Assert
        mockAlertRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
        Assert.Equal(alert, returnedAlert);
    }

    [Fact]
    public async Task Add_Alert_Success()
    {
        // Arrange
        var alert = new Alert();
        var mockAlertRepository = new Mock<IAlertRepository>();
        mockAlertRepository.Setup(repo => repo.AddAsync(alert)).Returns(Task.CompletedTask);

        // Act
        await mockAlertRepository.Object.AddAsync(alert);

        // Assert
        mockAlertRepository.Verify(repo => repo.AddAsync(alert), Times.Once);
    }
}