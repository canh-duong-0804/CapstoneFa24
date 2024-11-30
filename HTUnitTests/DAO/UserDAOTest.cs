using AutoMapper.Execution;
using BusinessObject.Models;
using DataAccess;
using HTUnitTests.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HTUnitTests.DAO
{
    public class UserDAOTest
    {
        private HealthTrackingDBContext _context;

        public UserDAOTest()
        {
            _context = CreateTestDatabase();
        }

        private HealthTrackingDBContext CreateTestDatabase()
        {
            var options = new DbContextOptionsBuilder<HealthTrackingDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new HealthTrackingDBContext(options);
            context.Database.EnsureCreated();

            if (!context.Members.Any())
            {
                var password = "TestPassword";
                UserDAO.Instance.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                var testMembers = new[]
                {
                new BusinessObject.Models.Member
                {
                    PhoneNumber = "0949267534",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = "Test1@gmail.com",
                    Dob = DateTime.Now,
                    Username = "TestUser1",
                    Status = true
                },
                new BusinessObject.Models.Member
                {
                    PhoneNumber = "Test_123456789",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = "existingemail@test.com",
                    Dob = DateTime.Now,
                    Username = "TestUser2",
                    Status = true
                }
            };

                context.Members.AddRange(testMembers);
                context.SaveChanges();

                if (!context.BodyMeasureChanges.Any())
                {
                    context.BodyMeasureChanges.Add(new BodyMeasureChange
                    {
                        MemberId = testMembers[0].MemberId,
                        Weight = 70.0,
                        DateChange = DateTime.UtcNow
                    });
                    context.SaveChanges();
                }
            }

            return context;
        }

        [Fact]
        public async Task Login_Successful_With_ValidCredentials()
        {
            // Arrange
            var password = "TestPassword";
            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "0949267534"
            };

            // Act
            // Assuming UserDAO.Instance.Login method is set up to work with the in-memory database
            var result = await UserDAO.Instance.Login(loginRequest, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser1", result.Username);
        }

        /*[Fact]
        public async Task Login_Fails_With_InvalidCredentials()
        {
            // Arrange
            await GetDatabaseContext();
            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789"
            };

            // Act
            var result = await UserDAO.Instance.Login(loginRequest, "WrongPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_Fails_With_InvalidPhoneNumber()
        {
            // Arrange
            await GetDatabaseContext();
            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_1234567WrongPhoneNumber"
            };

            // Act
            var result = await UserDAO.Instance.Login(loginRequest, "TruePassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_Fails_With_InvalidPhoneNumberAndPassword()
        {
            // Arrange
            await GetDatabaseContext();
            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_1234567WrongPhoneNumber"
            };

            // Act
            var result = await UserDAO.Instance.Login(loginRequest, "WrongPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task IsUniqueEmail_ShouldReturnTrue_WhenEmailDoesNotExist()
        {
            // Arrange
            await GetDatabaseContext();
            string uniqueEmail = "uniqueemail@test.com";

            // Act
            var result = UserDAO.Instance.IsUniqueEmail(uniqueEmail);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsUniqueEmail_ShouldReturnFalse_WhenEmailExists()
        {
            // Arrange
            await GetDatabaseContext();

            // Act
            var result = UserDAO.Instance.IsUniqueEmail("existingemail@test.com");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsUniquePhonenumber_ShouldReturnTrue_WhenPhoneNumberDoesNotExist()
        {
            // Arrange
            await GetDatabaseContext();
            string uniquePhoneNumber = "09498214124";

            // Act
            var result = UserDAO.Instance.IsUniquePhonenumber(uniquePhoneNumber);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsUniquePhonenumber_ShouldReturnFalse_WhenPhoneNumberExists()
        {
            // Arrange
            await GetDatabaseContext();

            // Act
            var result = UserDAO.Instance.IsUniquePhonenumber("Test_123456789");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetMemberByIdAsync_ShouldReturnMember_WhenMemberExists()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var existingMember = await context.Members.FirstOrDefaultAsync(m => m.PhoneNumber == "Test_123456789");
            Assert.NotNull(existingMember);

            // Act
            var result = await UserDAO.Instance.GetMemberByIdAsync(existingMember.MemberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingMember.MemberId, result.MemberId);
            Assert.Equal(existingMember.Username, result.Username);
            Assert.Equal(existingMember.Email, result.Email);

            Assert.NotNull(result.BodyMeasureChanges);
            Assert.Single(result.BodyMeasureChanges);
            Assert.Equal(70.0, result.BodyMeasureChanges.First().Weight);
        }

        [Fact]
        public async Task GetMemberByIdAsync_ShouldReturnNull_WhenMemberDoesNotExist()
        {
            // Arrange
            await GetDatabaseContext();
            int nonExistentMemberId = 999;

            // Act
            var result = await UserDAO.Instance.GetMemberByIdAsync(nonExistentMemberId);

            // Assert
            Assert.Null(result);
        }*/
    }
}