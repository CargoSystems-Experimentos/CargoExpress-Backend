using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class EntrepreneurUnitTest
{
    /*
    [Fact]
    public async Task GetAll_Entrepreneur_Success()
    {
        // Arrange
        var userEntrepreneur1 = new User("lucho@gmail.com", "contra1234567");
        var entrepreneur1 = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur1);

        var userEntrepreneur2 = new User("carlos@gmail.com", "contra1234567");
        var entrepreneur2 = new Entrepreneur("Carlos Perez", "986559214", "20000000003", "Av. Lima 123", "logo2.com/image.jpeg", 2, userEntrepreneur2);

        var entrepreneurs = new List<Entrepreneur> { entrepreneur1, entrepreneur2 };

        var mockEntrepreneurRepository = new Mock<IEntrepreneurRepository>();
        mockEntrepreneurRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(entrepreneurs);

        // Act
        var returnedEntrepreneurs = await mockEntrepreneurRepository.Object.ListAsync();

        // Assert
        mockEntrepreneurRepository.Verify(repo => repo.ListAsync(), Times.Once);
        Assert.Equal(entrepreneurs, returnedEntrepreneurs);
        Assert.Equal(2, returnedEntrepreneurs.Count());
    }
  
    [Fact]
    public async Task GetById_Entrepreneur_Success()
    {
        // Arrange
        int validId = 1;
        int invalidId = 0;
        var userEntrepreneur1 = new User("lucho@gmail.com", "contra1234567");
        var entrepreneur = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur1);
        var mockEntrepreneurRepository = new Mock<IEntrepreneurRepository>();
        mockEntrepreneurRepository.Setup(repo => repo.FindByIdAsync(validId)).ReturnsAsync(entrepreneur);
        mockEntrepreneurRepository.Setup(repo => repo.FindByIdAsync(invalidId)).ReturnsAsync((Entrepreneur)null);
        // Act
        var returnedEntrepreneur = await mockEntrepreneurRepository.Object.FindByIdAsync(validId);
        var returnedNullEntrepreneur = await mockEntrepreneurRepository.Object.FindByIdAsync(invalidId);

        // Assert
        mockEntrepreneurRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
        mockEntrepreneurRepository.Verify(repo => repo.FindByIdAsync(invalidId), Times.Once);
        Assert.Equal(entrepreneur, returnedEntrepreneur);
        Assert.Null(returnedNullEntrepreneur);
    }

    [Fact]
    public async Task Add_Entrepreneur_Success()
    {
        // Arrange
        var userEntrepreneur1 = new User("lucho@gmail.com", "contra1234567");
        var entrepreneur = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur1);
        var mockEntrepreneurRepository = new Mock<IEntrepreneurRepository>();
        mockEntrepreneurRepository.Setup(repo => repo.AddAsync(entrepreneur)).Returns(Task.CompletedTask);

        // Act
        await mockEntrepreneurRepository.Object.AddAsync(entrepreneur);

        // Assert
        mockEntrepreneurRepository.Verify(repo => repo.AddAsync(entrepreneur), Times.Once);
    }
    */
}