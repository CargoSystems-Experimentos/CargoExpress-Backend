using Moq;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;

namespace CargoExpress.UnitTests
{
    public class DriverUnitTest
    {
        /*
        [Fact]
        public async Task GetAll_Driver_Success()
        {
            // Arrange
            List<Driver> drivers = new List<Driver>
            {
                new Driver("Juan Perez", "12345678", "Brevete A1", "955123456"),
                new Driver("Pedro Sanchez", "87654321", "Brevete A2", "955765432")
            };
            var mockDriverRepository = new Mock<IDriverRepository>();
            mockDriverRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(drivers);

            // Act
            var returnedDrivers = await mockDriverRepository.Object.ListAsync();

            // Assert
            mockDriverRepository.Verify(repo => repo.ListAsync(), Times.Once);
            Assert.Equal(drivers, returnedDrivers);
            Assert.Equal(2, returnedDrivers.Count());
        }

        [Fact]
        public async Task GetById_Driver_Success()
        {
            // Arrange
            int validId = 1;
            int invalidId = 0;
            var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
            var mockDriverRepository = new Mock<IDriverRepository>();
            mockDriverRepository.Setup(repo => repo.FindByIdAsync(validId).Result).Returns(driver);
            mockDriverRepository.Setup(repo => repo.FindByIdAsync(invalidId).Result).Returns((Driver)null);

            // Act
            var returnedDriver = await mockDriverRepository.Object.FindByIdAsync(validId);
            var returnedNullDriver = await mockDriverRepository.Object.FindByIdAsync(invalidId);

            // Assert
            mockDriverRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
            mockDriverRepository.Verify(repo => repo.FindByIdAsync(invalidId), Times.Once);
            Assert.Equal(driver, returnedDriver);
            Assert.Null(returnedNullDriver);
        }
        
        [Fact]
        public async Task Add_Driver_Success()
        {
            // Arrange
            var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
            var mockDriverRepository = new Mock<IDriverRepository>();
            mockDriverRepository.Setup(repo => repo.AddAsync(driver)).Returns(Task.CompletedTask);

            // Act
            await mockDriverRepository.Object.AddAsync(driver);

            // Assert
            mockDriverRepository.Verify(repo => repo.AddAsync(driver), Times.Once);
        }

        [Fact]
        public void Update_Driver_Success()
        {
            // Arrange
            var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
            var mockDriverRepository = new Mock<IDriverRepository>();
            mockDriverRepository.Setup(repo => repo.Update(driver));

            // Update the driver properties
            driver.Name = "Pedro Sanchez";
            driver.Dni = "87654321";
            driver.License = "Brevete A2";
            driver.ContactNumber = "955765432";

            // Act
            mockDriverRepository.Object.Update(driver);

            // Assert
            mockDriverRepository.Verify(repo => repo.Update(driver), Times.Once);
            Assert.Equal("Pedro Sanchez", driver.Name);
            Assert.Equal("87654321", driver.Dni);
            Assert.Equal("Brevete A2", driver.License);
            Assert.Equal("955765432", driver.ContactNumber);
        }
            */
    }
}