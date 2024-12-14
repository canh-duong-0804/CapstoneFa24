using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HealthTrackingManageAPI.Controllers;
using HealthTrackingManageAPI.NewFolder.EsmsHelper;

namespace HTUnitTests.Controller
{
    public class SmsControllerTests
    {
        private readonly Mock<SpeedSMSService> _mockSmsService;
        private readonly SmsController _controller;

        public SmsControllerTests()
        {
            _mockSmsService = new Mock<SpeedSMSService>();
            _controller = new SmsController(_mockSmsService.Object);
        }

        [Fact]
        public void SendSMS_NormalCase_ReturnsOk()
        {
            // Arrange
            var request = new SmsRequest
            {
                Phones = "0987654321",
                Content = "Test message"
            };

            _mockSmsService.Setup(service => service.SendSMS(request.Phones, request.Content))
                .Returns("Success");

            // Act
            var result = _controller.SendSMS(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal("SMS sent successfully", ((dynamic)objectResult.Value).message);
        }

        [Fact]
        public void SendSMS_MissingPhone_ReturnsBadRequest()
        {
            // Arrange
            var request = new SmsRequest
            {
                Phones = null,
                Content = "Test message"
            };

            _mockSmsService.Setup(service => service.SendSMS(request.Phones, request.Content))
                .Returns((string)null);

            // Act
            var result = _controller.SendSMS(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal("SMS sending failed", objectResult.Value);
        }

        [Fact]
        public void SendSMS_MissingContent_ReturnsBadRequest()
        {
            // Arrange
            var request = new SmsRequest
            {
                Phones = "0987654321",
                Content = null
            };

            _mockSmsService.Setup(service => service.SendSMS(request.Phones, request.Content))
                .Returns((string)null);

            // Act
            var result = _controller.SendSMS(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal("SMS sending failed", objectResult.Value);
        }

        [Fact]
        public void SendSMS_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var request = new SmsRequest
            {
                Phones = "0987654321",
                Content = "Test message"
            };

            _mockSmsService.Setup(service => service.SendSMS(request.Phones, request.Content))
                .Throws(new Exception("Some error occurred"));

            // Act
            var result = _controller.SendSMS(request);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Some error occurred", ((dynamic)objectResult.Value).error);
        }
    }
}
