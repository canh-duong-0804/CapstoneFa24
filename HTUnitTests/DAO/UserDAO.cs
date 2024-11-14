using BusinessObject.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.DAO
{

    public class UserDAOTests
    {
        private readonly Mock<DbSet<Member>> _mockMembersDbSet;
        private readonly Mock<HealthTrackingDBContext> _mockContext;
        private readonly UserDAO _userDAO;

        public UserDAOTests()
        {
            _mockMembersDbSet = new Mock<DbSet<Member>>();
            _mockContext = new Mock<HealthTrackingDBContext>();

            _mockContext.Setup(c => c.Members).Returns(_mockMembersDbSet.Object);

            _userDAO = new UserDAO();
        }

        // Helper method to create a mock Member with a hashed password
        private Member CreateMockMember(string email, string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return new Member
            {
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }

        // Method to set up mock data in DbSet
        private void SetupMockData()
        {
            // Arrange mock data with multiple members
            var mockMembers = new List<Member>
            {
                CreateMockMember("test1@example.com", "Password123", out var hash1, out var salt1),
                CreateMockMember("user1@example.com", "User1Pass", out var hash2, out var salt2),
                CreateMockMember("user2@example.com", "User2Pass", out var hash3, out var salt3)
            };

            // Convert list to IQueryable and configure mock DbSet
            var data = mockMembers.AsQueryable();

            _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        [Theory]
        [InlineData("test1@example.com", "Password123", true)] // Valid credentials
        [InlineData("user1@example.com", "User1Pass", true)] // Another valid user
        [InlineData("user2@example.com", "WrongPassword", false)] // Invalid password for an existing user
        [InlineData("nonexistent@example.com", "Password123", false)] // Non-existent email
        public async Task Login_ValidatesCredentials_ReturnsExpectedResult(string email, string password, bool shouldSucceed)
        {
            // Arrange
            SetupMockData(); // Set up mock data with multiple members

            // Act
            var result = await _userDAO.Login(new Member { Email = email }, password);

            // Assert
            if (shouldSucceed)
            {
                Assert.NotNull(result);
                Assert.Equal(email, result.Email);
            }
            else
            {
                Assert.Null(result);
            }
        }
    }

        /* [Fact]
         public async Task Login_InvalidEmail_ReturnsNull()
         {
             // Arrange
             var email = "wrong@example.com";
             var password = "Password123";

             var data = new List<Member>().AsQueryable(); // Empty list simulating no user found

             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(data.Provider);
             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(data.Expression);
             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(data.ElementType);
             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

             _mockContext.Setup(c => c.Members).Returns(_mockMembersDbSet.Object);

             // Act
             var result = await _userDAO.Login(new Member { Email = email }, password);

             // Assert
             Assert.Null(result);
         }

         [Fact]
         public async Task Login_InvalidPassword_ReturnsNull()
         {
             // Arrange
             var email = "test@example.com";
             var correctPassword = "Password123";
             var wrongPassword = "WrongPassword";

             var mockMember = CreateMockMember(email, correctPassword, out var passwordHash, out var passwordSalt);
             var data = new List<Member> { mockMember }.AsQueryable();

             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(data.Provider);
             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(data.Expression);
             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(data.ElementType);
             _mockMembersDbSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

             _mockContext.Setup(c => c.Members).Returns(_mockMembersDbSet.Object);

             // Act
             var result = await _userDAO.Login(new Member { Email = email }, wrongPassword);

             // Assert
             Assert.Null(result);
         }*/
    }


