using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class VehicleUnitTest
{
    /*
    [Fact]
    public async Task GetAll_Vehicle_Success()
    {
        // Arrange
        List<Vehicle> vehicles = new List<Vehicle>
        {
            new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35),
            new Vehicle("Toyota", "B2C-345", "B2C-346", 6000, 40)
        };
        var vehicleDataMock = new Mock<IVehicleRepository>();
        vehicleDataMock.Setup(x => x.ListAsync().Result).Returns(vehicles);
        
        // Act
        var returnedVehicles = await vehicleDataMock.Object.ListAsync();
        
        // Assert
        vehicleDataMock.Verify(x => x.ListAsync(), Times.Once);
        Assert.Equal(vehicles, returnedVehicles);
        Assert.Equal(2, returnedVehicles.Count());
    }

    [Fact]
    public async Task GetById_Vehicle_Success()
    {
        // Arrange
        int validId = 1;
        int invalidId = 0;
        Vehicle vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
        var vehicleDataMock = new Mock<IVehicleRepository>();
        vehicleDataMock.Setup(x => x.FindByIdAsync(validId).Result).Returns(vehicle);
        vehicleDataMock.Setup(x => x.FindByIdAsync(invalidId).Result).Returns((Vehicle)null);
        
        // Act
        var returnedVehicle = await vehicleDataMock.Object.FindByIdAsync(validId);
        var returnedNullVehicle = await vehicleDataMock.Object.FindByIdAsync(invalidId);
        
        // Assert
        vehicleDataMock.Verify(x => x.FindByIdAsync(validId), Times.Once);
        vehicleDataMock.Verify(x => x.FindByIdAsync(invalidId), Times.Once);
        Assert.Equal(vehicle, returnedVehicle);
        Assert.Null(returnedNullVehicle);
    }
    
    [Fact]
    public async Task Add_Vehicle_Success()
    {
        //Arrange
       
        Vehicle vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
        var vehicleDataMock = new Mock<IVehicleRepository>();
        vehicleDataMock.Setup(x => x.AddAsync(vehicle)).Returns(Task.CompletedTask);
        //Act
        await vehicleDataMock.Object.AddAsync(vehicle);
        //Assert
        vehicleDataMock.Verify(x => x.AddAsync(vehicle), Times.Once);
    }
    
    [Fact]
    public void Update_Vehicle_Success()
    {
        // Arrange
        Vehicle vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
        var vehicleDataMock = new Mock<IVehicleRepository>();

        // Update the vehicle properties
        vehicle.Model = "Toyota";
        vehicle.Plate = "B2C-345";
        vehicle.TractorPlate = "B2C-346";
        vehicle.MaxLoad = 6000;
        vehicle.Volume = 40;

        vehicleDataMock.Setup(x => x.Update(vehicle));

        // Act
        vehicleDataMock.Object.Update(vehicle);

        // Assert
        vehicleDataMock.Verify(x => x.Update(vehicle), Times.Once);
        Assert.Equal("Toyota", vehicle.Model);
        Assert.Equal("B2C-345", vehicle.Plate);
        Assert.Equal("B2C-346", vehicle.TractorPlate);
        Assert.Equal(6000, vehicle.MaxLoad);
        Assert.Equal(40, vehicle.Volume);
    }
    */
}