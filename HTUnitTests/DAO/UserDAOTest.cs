using AutoMapper.Execution;
using BusinessObject.Models;
using DataAccess;
using HTUnitTests.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.DAO
{
    public class UserDAOTest : IClassFixture<DatabaseFixture>
    {
        private readonly HealthTrackingDBContext _context;

        public UserDAOTest(DatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Login_Successful_With_ValidCredentials()
        {
            // Arrange: Thêm dữ liệu test
            var password = "TestPassword";
            UserDAO.Instance.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email="Test@gmail.com",
                Dob=DateTime.Now,
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            await _context.SaveChangesAsync();

            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789"
            };

            // Act
            var result = await UserDAO.Instance.Login(loginRequest, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testMember.Username, result.Username);
        }

        [Fact]
        public async Task Login_Fails_With_InvalidCredentials()
        {
            // Arrange: Dữ liệu test
            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789"
            };

            // Act
            var result = await UserDAO.Instance.Login(loginRequest, "WrongPassword");

            // Assert
            Assert.Null(result);
        }

    }
}
