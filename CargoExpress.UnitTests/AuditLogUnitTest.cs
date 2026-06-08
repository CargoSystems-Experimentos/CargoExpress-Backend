using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class AuditLogUnitTest
{
    [Fact]
    public async Task GetAll_AuditLogs_Success()
    {
        // Arrange
        var auditLogs = new List<AuditLog>
        {
            new AuditLog("TRIPS", "CREATE", "{}", 1),
            new AuditLog("EXPENSES", "UPDATE", "{\"FuelAmount\":250}", 1)
        };
        var mockRepository = new Mock<IAuditLogRepository>();
        mockRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(auditLogs);

        // Act
        var returnedLogs = await mockRepository.Object.ListAsync();

        // Assert
        mockRepository.Verify(repo => repo.ListAsync(), Times.Once);
        Assert.Equal(auditLogs, returnedLogs);
        Assert.Equal(2, returnedLogs.Count());
    }

    [Fact]
    public async Task GetById_AuditLog_Success()
    {
        // Arrange
        var auditLog = new AuditLog("TRIPS", "CREATE", "{}", 1);
        Guid validId = auditLog.Id;
        Guid invalidId = Guid.NewGuid();
        var mockRepository = new Mock<IAuditLogRepository>();
        mockRepository.Setup(repo => repo.FindByIdAsync(validId)).ReturnsAsync(auditLog);
        mockRepository.Setup(repo => repo.FindByIdAsync(invalidId)).ReturnsAsync((AuditLog?)null);

        // Act
        var returnedLog = await mockRepository.Object.FindByIdAsync(validId);
        var returnedNull = await mockRepository.Object.FindByIdAsync(invalidId);

        // Assert
        mockRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
        mockRepository.Verify(repo => repo.FindByIdAsync(invalidId), Times.Once);
        Assert.Equal(auditLog, returnedLog);
        Assert.Null(returnedNull);
    }

    [Fact]
    public async Task Add_AuditLog_Success()
    {
        // Arrange
        var auditLog = new AuditLog("DRIVERS", "DELETE", "{}", 2);
        var mockRepository = new Mock<IAuditLogRepository>();
        mockRepository.Setup(repo => repo.AddAsync(auditLog)).Returns(Task.CompletedTask);

        // Act
        await mockRepository.Object.AddAsync(auditLog);

        // Assert
        mockRepository.Verify(repo => repo.AddAsync(auditLog), Times.Once);
    }

    [Fact]
    public async Task FindByEntrepreneurId_AuditLogs_Success()
    {
        // Arrange
        int entrepreneurId = 5;
        var auditLogs = new List<AuditLog>
        {
            new AuditLog("TRIPS", "CREATE", "{}", entrepreneurId),
            new AuditLog("ALERTS", "UPDATE", "{}", entrepreneurId)
        };
        var mockRepository = new Mock<IAuditLogRepository>();
        mockRepository.Setup(repo => repo.FindByEntrepreneurIdAsync(entrepreneurId)).ReturnsAsync(auditLogs);

        // Act
        var returnedLogs = await mockRepository.Object.FindByEntrepreneurIdAsync(entrepreneurId);

        // Assert
        mockRepository.Verify(repo => repo.FindByEntrepreneurIdAsync(entrepreneurId), Times.Once);
        Assert.Equal(auditLogs, returnedLogs);
        Assert.All(returnedLogs, log => Assert.Equal(entrepreneurId, log.EntrepreneurId));
    }

    [Fact]
    public async Task FindByEntrepreneurIdAndEntityType_AuditLogs_Success()
    {
        // Arrange
        int entrepreneurId = 3;
        string entityType = "EXPENSES";
        var auditLogs = new List<AuditLog>
        {
            new AuditLog(entityType, "CREATE", "{}", entrepreneurId),
            new AuditLog(entityType, "UPDATE", "{\"FuelAmount\":300}", entrepreneurId)
        };
        var mockRepository = new Mock<IAuditLogRepository>();
        mockRepository.Setup(repo => repo.FindByEntrepreneurIdAndEntityTypeAsync(entrepreneurId, entityType))
            .ReturnsAsync(auditLogs);

        // Act
        var returnedLogs = await mockRepository.Object.FindByEntrepreneurIdAndEntityTypeAsync(entrepreneurId, entityType);

        // Assert
        mockRepository.Verify(repo => repo.FindByEntrepreneurIdAndEntityTypeAsync(entrepreneurId, entityType), Times.Once);
        Assert.Equal(auditLogs, returnedLogs);
        Assert.All(returnedLogs, log => Assert.Equal(entityType, log.EntityType));
    }

    [Fact]
    public void Create_AuditLog_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var auditLog = new AuditLog("VEHICLES", "UPDATE", "{\"Model\":\"Toyota\"}", 7);

        // Assert
        Assert.Equal("VEHICLES", auditLog.EntityType);
        Assert.Equal("UPDATE", auditLog.Action);
        Assert.Equal("{\"Model\":\"Toyota\"}", auditLog.ModifiedFields);
        Assert.Equal(7, auditLog.EntrepreneurId);
        Assert.NotEqual(Guid.Empty, auditLog.Id);
    }
}
