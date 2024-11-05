using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HealthTrackingManageAPI.Controllers;
using Repository.IRepo;
using BusinessObject.Models;
using BusinessObject.Dto.Staff;
using HealthTrackingManageAPI;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace HealthTrackingUnitTests.ControllerAPI.Controller
{
    [TestFixture]
    public class StaffControllerTests
    {
        private Mock<IStaffRepository> _staffRepo;
        private Mock<IConfiguration> _configuration;
        private Mock<HealthTrackingDBContext> _context;
        private Mock<IOptionsMonitor<AppSettingsKey>> _optionsMonitor;
        private StaffController _controller;

        [SetUp]
        public void SetUp()
        {
            _staffRepo = new Mock<IStaffRepository>();
            _configuration = new Mock<IConfiguration>();
            _context = new Mock<HealthTrackingDBContext>();
            _optionsMonitor = new Mock<IOptionsMonitor<AppSettingsKey>>();

           
            _optionsMonitor.Setup(x => x.CurrentValue).Returns(new AppSettingsKey());

            _controller = new StaffController(_context.Object, _configuration.Object, _staffRepo.Object, _optionsMonitor.Object);
        }



        [Test]
        public async Task GetAllAccountStaffForAdmin_WithValidPage_ReturnsOkResult()
        {
            // Arrange
            int? page = 1;
            int expectedCurrentPage = 1;
            int expectedPageSize = 5;
            int totalStaffs = 10;

            var expectedStaffs = new List<AllStaffsResponseDTO>
    {
        new AllStaffsResponseDTO
        {
            StaffId = 1,
            FullName = "Test Staff 1",
            PhoneNumber = "1234567890",
            Description = "Test Description 1",
            StaffImage = "image1.jpg",
            Email = "staff1@test.com",
            Role = 1,
            StartWorkingDate = DateTime.Now,
            Status = true
        }
    };

            _staffRepo.Setup(x => x.GetTotalStaffCountAsync())
                      .ReturnsAsync(totalStaffs);

            _staffRepo.Setup(x => x.GetAllAccountStaffsAsync(
                It.Is<int>(p => p == expectedCurrentPage),
                It.Is<int>(ps => ps == expectedPageSize)))
                .ReturnsAsync(expectedStaffs);

            // Act
            var result = await _controller.GetAllAccountStaffForAdmin(page);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult but got null or another result type.");

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Expected OkObjectResult but got null.");

            var response = JObject.FromObject(okResult.Value);

            Assert.Multiple(() =>
            {
                Assert.That((int)response["CurrentPage"], Is.EqualTo(expectedCurrentPage));
                Assert.That((int)response["PageSize"], Is.EqualTo(expectedPageSize));
                Assert.That((int)response["TotalPages"], Is.EqualTo(2)); // 10/5 = 2 pages

                var returnedStaffs = response["staffs"].ToObject<List<AllStaffsResponseDTO>>();
                Assert.That(returnedStaffs, Has.Count.EqualTo(1));
                Assert.That(returnedStaffs[0].StaffId, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetAllAccountStaffForAdmin_WithNegativePage_AdjustsToPageOne()
        {
            // Arrange
            int? page = -1;
            int expectedCurrentPage = 1;
            int expectedPageSize = 5;
            int totalStaffs = 10;

            var expectedStaffs = new List<AllStaffsResponseDTO>
    {
        new AllStaffsResponseDTO
        {
             StaffId = 1,
            FullName = "Test Staff 1",
            PhoneNumber = "1234567890",
            Description = "Test Description 1",
            StaffImage = "image1.jpg",
            Email = "staff1@test.com",
            Role = 1,
            StartWorkingDate = DateTime.Now,
            Status = true
        }
    };

            _staffRepo.Setup(x => x.GetTotalStaffCountAsync())
                      .ReturnsAsync(totalStaffs);

            _staffRepo.Setup(x => x.GetAllAccountStaffsAsync(
                It.Is<int>(p => p == expectedCurrentPage),
                It.Is<int>(ps => ps == expectedPageSize)))
                .ReturnsAsync(expectedStaffs);

            // Act
            var result = await _controller.GetAllAccountStaffForAdmin(page);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult but got null or another result type.");

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Expected OkObjectResult but got null.");

            
            var response = JObject.FromObject(okResult.Value);

            Assert.That((int)response["CurrentPage"], Is.EqualTo(expectedCurrentPage), "CurrentPage did not adjust to 1 as expected.");
            Assert.That((int)response["PageSize"], Is.EqualTo(expectedPageSize));
            Assert.That((int)response["TotalPages"], Is.EqualTo(2)); // 10/5 = 2 pages

            var returnedStaffs = response["staffs"].ToObject<List<AllStaffsResponseDTO>>();
            Assert.That(returnedStaffs, Has.Count.EqualTo(1));
            Assert.That(returnedStaffs[0].StaffId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllAccountStaffForAdmin_WithPageTooHigh_AdjustsToLastPage()
        {
            // Arrange
            int? page = 100;
            int totalStaffs = 8;
            int expectedCurrentPage = 2; // With 8 items and pageSize 5, last page should be 2
            int expectedPageSize = 5;

            var expectedStaffs = new List<AllStaffsResponseDTO>
    {
        new AllStaffsResponseDTO
        {
            StaffId = 1,
            FullName = "Test Staff 1",
            PhoneNumber = "1234567890",
            Description = "Test Description 1",
            StaffImage = "image1.jpg",
            Email = "staff1@test.com",
            Role = 1,
            StartWorkingDate = DateTime.Now,
            Status = true
        }
    };

            _staffRepo.Setup(x => x.GetTotalStaffCountAsync())
                      .ReturnsAsync(totalStaffs);

            _staffRepo.Setup(x => x.GetAllAccountStaffsAsync(
                It.Is<int>(p => p == expectedCurrentPage),
                It.Is<int>(ps => ps == expectedPageSize)))
                .ReturnsAsync(expectedStaffs);

            // Act
            var result = await _controller.GetAllAccountStaffForAdmin(page);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            var response = JObject.FromObject(okResult.Value);
            Assert.That((int)response["CurrentPage"], Is.EqualTo(expectedCurrentPage));
        }


        [Test]
        public async Task GetAllAccountStaffForAdmin_WithNoStaff_ReturnsNotFound()
        {
            // Arrange
            int? page = 1;
            int totalStaffs = 0;

            _staffRepo.Setup(x => x.GetTotalStaffCountAsync())
                      .ReturnsAsync(totalStaffs);

            _staffRepo.Setup(x => x.GetAllAccountStaffsAsync(
                It.IsAny<int>(),
                It.IsAny<int>()))
                .ReturnsAsync(new List<AllStaffsResponseDTO>());

            // Act
            var result = await _controller.GetAllAccountStaffForAdmin(page);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("No staff found."));
        }


        [Test]
        public async Task GetAllAccountStaffForAdmin_WithNullPage_DefaultsToPageOne()
        {
            // Arrange
            int? page = null;
            int expectedCurrentPage = 1;
            int expectedPageSize = 5;
            int totalStaffs = 10;

            var expectedStaffs = new List<AllStaffsResponseDTO>
    {
        new AllStaffsResponseDTO
        {
             StaffId = 1,
            FullName = "Test Staff 1",
            PhoneNumber = "1234567890",
            Description = "Test Description 1",
            StaffImage = "image1.jpg",
            Email = "staff1@test.com",
            Role = 1,
            StartWorkingDate = DateTime.Now,
            Status = true
        }
    };

            _staffRepo.Setup(x => x.GetTotalStaffCountAsync())
                      .ReturnsAsync(totalStaffs);

            _staffRepo.Setup(x => x.GetAllAccountStaffsAsync(
                It.Is<int>(p => p == expectedCurrentPage),
                It.Is<int>(ps => ps == expectedPageSize)))
                .ReturnsAsync(expectedStaffs);

            // Act
            var result = await _controller.GetAllAccountStaffForAdmin(page);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            var response = JObject.FromObject(okResult.Value);
            Assert.That((int)response["CurrentPage"], Is.EqualTo(expectedCurrentPage));
        }


        [Test]
        public async Task GetAllAccountStaffForAdmin_SmallTotalStaffs_AdjustsPageSize()
        {
            // Arrange
            int? page = 1;
            int totalStaffs = 3; 
            int expectedPageSize = 3; 

            var expectedStaffs = new List<AllStaffsResponseDTO>
    {
        new AllStaffsResponseDTO { StaffId = 1 },
        new AllStaffsResponseDTO { StaffId = 2 },
        new AllStaffsResponseDTO { StaffId = 3 }
    };

            _staffRepo.Setup(x => x.GetTotalStaffCountAsync())
                      .ReturnsAsync(totalStaffs);

            _staffRepo.Setup(x => x.GetAllAccountStaffsAsync(
                It.IsAny<int>(),
                It.IsAny<int>()))
                .ReturnsAsync(expectedStaffs);

            // Act
            var result = await _controller.GetAllAccountStaffForAdmin(page);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            var response = JObject.FromObject(okResult.Value);
            Assert.That((int)response["PageSize"], Is.EqualTo(expectedPageSize));
            Assert.That((int)response["TotalPages"], Is.EqualTo(1));
        }

    }
}