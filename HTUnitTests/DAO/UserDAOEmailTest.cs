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
    public class UserDAOEmailTest : IClassFixture<DatabaseFixture>
    {
        private readonly HealthTrackingDBContext _context;

        public UserDAOEmailTest(DatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        // Test case for unique email
        [Fact]
        public void IsUniqueEmail_ReturnsTrue_WhenEmailIsUnique()
        {
            // Arrange: Add a member with a specific email
            var email = "Test_uniqueemail@example.com";
            var testMember = new BusinessObject.Models.Member
            {
                Email = email,
                PhoneNumber = "Test_123456789",
                Dob=DateTime.Now,
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { },
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            _context.SaveChanges();

            // Act: Check if the email is unique
            var result = UserDAO.Instance.IsUniqueEmail(email);

            // Assert: The result should be false since the email is already in use
            Assert.False(result);
        }

        // Test case for unique email
        [Fact]
        public void IsUniqueEmail_ReturnsTrue_WhenEmailIsNotInUse()
        {
            // Arrange: Use a new email that is not in the database
            var email = "Test_newuniqueemail@example.com";

            // Act: Check if the email is unique
            var result = UserDAO.Instance.IsUniqueEmail(email);

            // Assert: The result should be true since the email is not in use
            Assert.True(result);
        }

        // Test case for duplicate email
        [Fact]
        public void IsUniqueEmail_ReturnsFalse_WhenEmailIsNotUnique()
        {
            // Arrange: Add a member with a specific email
            var email = "Test_duplicateemail@example.com";
            var testMember = new BusinessObject.Models.Member
            {
                Email = email,
                PhoneNumber = "Test_123456789",
                PasswordHash = new byte[] { },
                Dob=DateTime.Now,
                PasswordSalt = new byte[] { },
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            _context.SaveChanges();

            // Act: Check if the email is unique
            var result = UserDAO.Instance.IsUniqueEmail(email);

            // Assert: The result should be false since the email is already in use
            Assert.False(result);
        }

        // Test case for unique phone number
        [Fact]
        public void IsUniquePhonenumber_ReturnsTrue_WhenPhoneNumberIsUnique()
        {
            // Arrange: Add a member with a specific phone number
            var phoneNumber = "1234567890";
            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = phoneNumber,
                Email = "Test_123456789@example.com",
                Dob = DateTime.Now,
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { },
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            _context.SaveChanges();

            // Act: Check if the phone number is unique
            var result = UserDAO.Instance.IsUniquePhonenumber(phoneNumber);

            // Assert: The result should be false since the phone number is already in use
            Assert.False(result);
        }

        // Test case for unique phone number
        [Fact]
        public void IsUniquePhonenumber_ReturnsTrue_WhenPhoneNumberIsNotInUse()
        {
            // Arrange: Use a new phone number that is not in the database
            var phoneNumber = "9876543210";

            // Act: Check if the phone number is unique
            var result = UserDAO.Instance.IsUniquePhonenumber(phoneNumber);

            // Assert: The result should be true since the phone number is not in use
            Assert.True(result);
        }

        // Test case for duplicate phone number
        [Fact]
        public void IsUniquePhonenumber_ReturnsFalse_WhenPhoneNumberIsNotUnique()
        {
            // Arrange: Add a member with a specific phone number
            var phoneNumber = "1112223333";
            var testMember = new BusinessObject.Models.Member
            {
                PhoneNumber = phoneNumber,
                Email = "Test_1112223333@example.com",
                Dob = DateTime.Now,
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { },
                Username = "TestUser"
            };

            _context.Members.Add(testMember);
            _context.SaveChanges();

            // Act: Check if the phone number is unique
            var result = UserDAO.Instance.IsUniquePhonenumber(phoneNumber);

            // Assert: The result should be false since the phone number is already in use
            Assert.False(result);
        }

        /*// Test case to ensure exception handling for database errors (e.g., connection issues)
        [Fact]
        public void IsUniqueEmail_ThrowsException_WhenDatabaseFails()
        {
            // Arrange: Simulate a failure in the database context (you can mock this if needed)
            var email = "error@example.com";

            // Act & Assert: Simulate database failure and expect an exception to be thrown
            Assert.Throws<Exception>(() => UserDAO.Instance.IsUniqueEmail(email));
        }

        [Fact]
        public void IsUniquePhonenumber_ThrowsException_WhenDatabaseFails()
        {
            // Arrange: Simulate a failure in the database context (you can mock this if needed)
            var phoneNumber = "errorPhone";

            // Act & Assert: Simulate database failure and expect an exception to be thrown
            Assert.Throws<Exception>(() => UserDAO.Instance.IsUniquePhonenumber(phoneNumber));
        }*/
    }
}
