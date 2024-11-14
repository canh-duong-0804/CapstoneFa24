/*using NUnit.Framework;
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







        //viết test cho method [HttpGet("get-all-account-staff-for-admin")]
        // public async Task<IActionResult> GetAllAccountStaffForAdmin([FromQuery] int? page)

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





        //viết test cho method[HttpGet("get-account-staff-for-staff-by-id/{id}")]
        //public async Task<IActionResult> GetAccountPersonalForStaffById(int id)
        [Test]
        public async Task GetAccountStaffById_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            int staffId = 1;
            var staff = new GetStaffByIdResponseDTO { StaffId = staffId, FullName = "John Doe", Email = "johndoe@example.com" };
            _staffRepo.Setup(repo => repo.GetAccountStaffForAdminByIdAsync(staffId)).ReturnsAsync(staff);

            // Act
            var result = await _controller.GetAccountStaffById(staffId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult when staff exists.");
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(staff));
        }

        [Test]
        public async Task GetAccountStaffById_ReturnsNotFoundResult_WhenStaffDoesNotExist()
        {
            // Arrange
            int staffId = 1;
            _staffRepo.Setup(repo => repo.GetAccountStaffForAdminByIdAsync(staffId)).ReturnsAsync((GetStaffByIdResponseDTO)null);

            // Act
            var result = await _controller.GetAccountStaffById(staffId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>(), "Expected NotFoundObjectResult when staff does not exist.");
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("staff not found."));
        }

        [Test]
        public async Task GetAccountStaffById_ReturnsInternalServerError_WhenRepositoryThrowsException()
        {
            // Arrange
            int staffId = 1;
            _staffRepo.Setup(repo => repo.GetAccountStaffForAdminByIdAsync(staffId)).ThrowsAsync(new System.Exception("Database failure"));

            // Act
            var result = await _controller.GetAccountStaffById(staffId);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>(), "Expected ObjectResult for Internal Server Error.");
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500), "Expected status code 500 for Internal Server Error.");
            Assert.That(objectResult.Value, Is.EqualTo("An error occurred while fetching the staff data."));
        }











        *//* viết test cho method[HttpGet("get-account-staff-for-admin-by-id/{id}")]
         public async Task<IActionResult> GetAccountStaffById(int id)*//*


        [Test]
        public async Task GetAccountPersonalForStaffById_ReturnsOkResult_WhenStaffExists()
        {
            // Arrange
            int staffId = 1;
            var staff = new GetStaffPersonalByIdResponseDTO { StaffId = staffId, FullName = "John Doe", Email = "johndoe@example.com" };
            _staffRepo.Setup(repo => repo.GetAccountPersonalForStaffByIdAsync(staffId)).ReturnsAsync(staff);

            // Act
            var result = await _controller.GetAccountPersonalForStaffById(staffId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult when staff exists.");
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(staff));
        }

        [Test]
        public async Task GetAccountPersonalForStaffById_ReturnsNotFoundResult_WhenStaffDoesNotExist()
        {
            // Arrange
            int staffId = 1;
            _staffRepo.Setup(repo => repo.GetAccountPersonalForStaffByIdAsync(staffId)).ReturnsAsync((GetStaffPersonalByIdResponseDTO)null);

            // Act
            var result = await _controller.GetAccountPersonalForStaffById(staffId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>(), "Expected NotFoundObjectResult when staff does not exist.");
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("staff not found."));
        }

        [Test]
        public async Task GetAccountPersonalForStaffById_ReturnsInternalServerError_WhenRepositoryThrowsException()
        {
            // Arrange
            int staffId = 1;
            _staffRepo.Setup(repo => repo.GetAccountPersonalForStaffByIdAsync(staffId)).ThrowsAsync(new System.Exception("Database failure"));

            // Act
            var result = await _controller.GetAccountPersonalForStaffById(staffId);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>(), "Expected ObjectResult for Internal Server Error.");
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500), "Expected status code 500 for Internal Server Error.");
            Assert.That(objectResult.Value, Is.EqualTo("An error occurred while fetching the staff data."));
        }


    }
}*/