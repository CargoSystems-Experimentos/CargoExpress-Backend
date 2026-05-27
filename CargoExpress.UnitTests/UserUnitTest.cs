using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.IAM.Domain.Repositories;
using Moq;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Repositories;

namespace CargoExpress.UnitTests
{
    public class UserUnitTest
    {
        [Fact]
        public async Task GetAll_User_Success()
        {
            // Arrange
            var users = new List<User>
            {
                new User("juan@gmail.com", "contra1234567"),
                new User("pedro@gmail.com", "contra1234567")
            };
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.ListAsync().Result).Returns(users);
            
            // Act
            var returnedUsers = await mockUserRepository.Object.ListAsync();
            
            // Assert
            mockUserRepository.Verify(repo => repo.ListAsync(), Times.Once);
            Assert.Equal(users, returnedUsers);
            Assert.Equal(2, returnedUsers.Count());
            
        }

        [Fact]
        public async Task GetById_User_Success()
        {
            // Arrange
            int validId = 1;
            int invalidId = 0;
            var user = new User("juan@gmail.com", "contra1234567");
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByIdAsync(validId).Result).Returns(user);
            mockUserRepository.Setup(repo => repo.FindByIdAsync(invalidId).Result).Returns((User)null);

            // Act
            var returnedUser = await mockUserRepository.Object.FindByIdAsync(validId);
            var returnedNullUser = await mockUserRepository.Object.FindByIdAsync(invalidId);

            // Assert
            mockUserRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
            mockUserRepository.Verify(repo => repo.FindByIdAsync(invalidId), Times.Once);
            
            Assert.Equal(user, returnedUser);
            Assert.Null(returnedNullUser);
        }
        
        
        [Fact]
        public async Task Add_User_Success()
        {
            // Arrange
            var user = new User("juan@gmail.com", "contra1234567");
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.AddAsync(user)).Returns(Task.CompletedTask);

            // Act
            await mockUserRepository.Object.AddAsync(user);

            // Assert
            mockUserRepository.Verify(repo => repo.AddAsync(user), Times.Once);
        }
    }
}