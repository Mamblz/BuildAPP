using DesktopProgram.Services;
using DesktopProgram.Models;
using DesktopProgram.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Linq;

namespace BuildProgram.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthServiceTests()
        {
            // Используем InMemoryDatabase для изолированных тестов
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AuthDB")
                .Options;

            _context = new ApplicationDbContext(options);
            _authService = new AuthService(_context);
        }

        [Fact]
        public void Register_ValidUser_ReturnsTrue()
        {
            // Act
            bool result = _authService.Register("newUser", "new@mail.com", "Password123!");

            // Assert
            Assert.True(result);
            Assert.Equal(1, _context.Users.Count());
        }

        [Fact]
        public void Register_DuplicateUser_ReturnsFalse()
        {
            // Arrange
            _authService.Register("existingUser", "existing@mail.com", "Password123!");

            // Act
            bool result = _authService.Register("existingUser", "existing@mail.com", "Password123!");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            _authService.Register("testUser", "test@mail.com", "Password123!");

            // Act
            var user = _authService.Login("testUser", "Password123!");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("testUser", user.Username);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsNull()
        {
            // Arrange
            _authService.Register("testUser", "test@mail.com", "Password123!");

            // Act
            var user = _authService.Login("testUser", "WrongPassword!");

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void HashPassword_ReturnsConsistentHash()
        {
            // Arrange
            string password = "TestPassword123";

            // Act
            string hash1 = _authService.GetHashedPassword(password);
            string hash2 = _authService.GetHashedPassword(password);

            // Assert
            Assert.Equal(hash1, hash2);
            Assert.NotEqual(password, hash1); // Хеш не должен совпадать с исходным паролем
        }
    }
}