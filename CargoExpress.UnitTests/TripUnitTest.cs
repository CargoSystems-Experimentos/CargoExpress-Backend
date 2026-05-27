using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using Moq;

namespace CargoApp.UnitTests;

public class TripUnitTest
{
    /*
    [Fact]
    public async Task GetAll_Trip_Success()
    {
        // Arrange
        
        var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
        var vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
        var userClient = new User("juan@gmail.com", "contra123");
        var userEntrepreneur = new User("lucho@gmail.com", "contra123");
        var client = new Client("Juan Perez", "986559113", "20000000001", "Av. Lima 123", 1, userClient);
        var entrepreneur = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur);
        
        var trips = new List<Trip>
        {
            new Trip("Viaje 1", "Tecnologia", 500, 
                "Av. San Borja Sur", new DateTime(2024, 07, 05), 
                "Av. San Borja Norte", new DateTime(2024, 07, 06), 
                1, 1, 1, 1, driver, vehicle, client, entrepreneur),
            new Trip("Viaje 2", "Alimentos", 1000, 
                "Calle Las Begonias 730", new DateTime(2024, 07, 08), 
                "Av. Vicus I-92", new DateTime(2024, 08, 09), 
                1, 1, 1, 1, driver, vehicle, client, entrepreneur),
        };
        var mockTripRepository = new Mock<ITripRepository>();
        mockTripRepository.Setup(repo => repo.ListAsync().Result).Returns(trips);
        
        // Act
        var returnedTrips = await mockTripRepository.Object.ListAsync();
        
        // Assert
        mockTripRepository.Verify(repo => repo.ListAsync(), Times.Once);
        Assert.Equal(trips, returnedTrips);
        Assert.Equal(2, returnedTrips.Count());
    }

    [Fact]
    public async Task GetById_Trip_Success()
    {
        // Arrange
        int validId = 1;
        int invalidId = 0;

        var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
        var vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
        var userClient = new User("juan@gmail.com", "contra123");
        var userEntrepreneur = new User("lucho@gmail.com", "contra123");
        var client = new Client("Juan Perez", "986559113", "20000000001", "Av. Lima 123", 1, userClient);
        var entrepreneur = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur);

        var trip = new Trip("Viaje 1", "Tecnologia", 500, 
            "Av. San Borja Sur", new DateTime(2024, 07, 05), 
            "Av. San Borja Norte", new DateTime(2024, 07, 06), 
            1, 1, 1, 1, driver, vehicle, client, entrepreneur);
        
        var mockTripRepository = new Mock<ITripRepository>();
        mockTripRepository.Setup(repo => repo.FindByIdAsync(validId).Result).Returns(trip);
        mockTripRepository.Setup(repo => repo.FindByIdAsync(invalidId).Result).Returns((Trip)null);
        
        // Act
        var returnedTrip = await mockTripRepository.Object.FindByIdAsync(validId);
        var returnedNullTrip = await mockTripRepository.Object.FindByIdAsync(invalidId);
        
        // Assert
        mockTripRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
        mockTripRepository.Verify(repo => repo.FindByIdAsync(invalidId), Times.Once);
        Assert.Equal(trip, returnedTrip);
        Assert.Null(returnedNullTrip);
    }

    [Fact]
    public async Task Add_Trip_Success()
    {
        //Arrange
        var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
        var vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
       var userClient = new User("juan@gmail.com", "contra123");
        var userEntrepreneur = new User("lucho@gmail.com", "contra123");
        var client = new Client("Juan Perez", "986559113", "20000000001", "Av. Lima 123", 1, userClient);
        var entrepreneur = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur);
        
        var trip = new Trip("Viaje 1", "Tecnologia", 500, 
            "Av. San Borja Sur", new DateTime(2024, 07, 05), 
            "Av. San Borja Norte", new DateTime(2024, 07, 06), 
            1, 1, 1, 1, driver, vehicle, client, entrepreneur);
        
        var mockTripRepository = new Mock<ITripRepository>();
        mockTripRepository.Setup(repo => repo.AddAsync(trip)).Returns(Task.CompletedTask);
        
        //Act
        await mockTripRepository.Object.AddAsync(trip);
        
        //Assert
        mockTripRepository.Verify(repo => repo.AddAsync(trip), Times.Once);
    }
    
    [Fact]
    public void Update_Trip_Success()
    {
        // Arrange
        var driver = new Driver("Juan Perez", "12345678", "Brevete A1", "955123456");
        var vehicle = new Vehicle("Volkswagen", "A1B-234", "A1B-235", 5000, 35);
        var userClient = new User("juan@gmail.com", "contra123");
        var userEntrepreneur = new User("lucho@gmail.com", "contra123");
        var client = new Client("Juan Perez", "986559113", "20000000001", "Av. Lima 123", 1, userClient);
        var entrepreneur = new Entrepreneur("Lucho Vega", "986559213", "20000000002", "Av. Peru 123", "logo.com/image.jpeg", 1, userEntrepreneur);

        var trip = new Trip("Viaje 1", "Tecnologia", 500,
            "Av. San Borja Sur", new DateTime(2024, 07, 05),
            "Av. San Borja Norte", new DateTime(2024, 07, 06),
            1, 1, 1, 1, driver, vehicle, client, entrepreneur);

        var mockTripRepository = new Mock<ITripRepository>();
        mockTripRepository.Setup(repo => repo.Update(trip));

        // Act
        mockTripRepository.Object.Update(trip);

        // Assert
        mockTripRepository.Verify(repo => repo.Update(trip), Times.Once);
    } 
    */
}