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
    public class UserLoginDAOTest : IClassFixture<DatabaseFixture>
    {
        private readonly HealthTrackingDBContext _context;

        public UserLoginDAOTest(DatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Login_Successful_With_ValidCredentials()
        {
            // Arrange: Thêm dữ liệu test
            var password = "Test_CorrectPassword";
            UserDAO.Instance.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_0949267534",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = "Test@gmail.com",
                Dob = DateTime.Now,
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            await _context.SaveChangesAsync();

            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_0949267534"
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
                PhoneNumber = "Test_WrongPhoneNumber"
            };

            // Act
            var result = await UserDAO.Instance.Login(loginRequest, "Test_WrongPassword");

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task Login_Fails_With_NonExistentUser()
        {
            // Arrange: User does not exist in the database
            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_WrongPhoneNumber"
            };

            // Act: Call the login method
            var result = await UserDAO.Instance.Login(loginRequest, "Test_Password");

            // Assert: Verify that login fails and returns null
            Assert.Null(result);
        }

        // Test case for login failure with empty password
        [Fact]
        public async Task Login_Fails_With_EmptyPassword()
        {
            // Arrange: Add test data to the database
            var password = "TestPassword";
            UserDAO.Instance.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = "Test@gmail.com",
                Dob = DateTime.Now,
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            await _context.SaveChangesAsync();

            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789"
            };

            // Act: Call the login method with an empty password
            var result = await UserDAO.Instance.Login(loginRequest, string.Empty);

            // Assert: Verify that login fails and returns null
            Assert.Null(result);
        }

        // Test case for login failure with empty phone number
        [Fact]
        public async Task Login_Fails_With_EmptyPhoneNumber()
        {
            // Arrange: Add test data to the database
            var password = "TestPassword";
            UserDAO.Instance.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = "Test@gmail.com",
                Dob = DateTime.Now,
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            await _context.SaveChangesAsync();

            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = string.Empty // Empty phone number
            };

            // Act: Call the login method with empty phone number
            var result = await UserDAO.Instance.Login(loginRequest, "TestPassword");

            // Assert: Verify that login fails and returns null
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_Fails_With_EmptyPhoneNumberAndPassword()
        {
            // Arrange: Add test data to the database
            var password = "TestPassword";
            UserDAO.Instance.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = "Test_123456789",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = "Test@gmail.com",
                Dob = DateTime.Now,
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            await _context.SaveChangesAsync();

            var loginRequest = new BusinessObject.Models.Member
            {
                PhoneNumber = string.Empty // Empty phone number
            };

            // Act: Call the login method with empty phone number
            var result = await UserDAO.Instance.Login(loginRequest, "");

            // Assert: Verify that login fails and returns null
            Assert.Null(result);
        }


    }
}