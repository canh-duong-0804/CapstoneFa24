using AutoMapper;
using BusinessObject.Dto.MealMember;
using BusinessObject.Models;
using BusinessObject;
using HealthTrackingManageAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Repository.IRepo;

namespace HTUnitTests.Controller
{
    public class GetAllMealMembersInMealMemberControllerTest
    {
        private readonly Mock<IMealMemberRepository> _mockRepository;
        private readonly MealMemberController _controller;

        public GetAllMealMembersInMealMemberControllerTest()
        {
            _mockRepository = new Mock<IMealMemberRepository>();
            _controller = new MealMemberController(_mockRepository.Object,null);
        }

        [Theory]
        [InlineData("1", true)]
        public async Task GetAllMealMembers_Authenticated_ReturnsOk(string memberId, bool repositoryHasData)
        {
            // Arrange
            SetUserClaim(memberId);

            var mealMembers = repositoryHasData
                ? new List<MealMember> { new MealMember { MealMemberId = 1, NameMealMember = "Test Member" } }
                : new List<MealMember>();

            _mockRepository.Setup(repo => repo.GetAllMealMembersAsync(It.IsAny<int>()))
                .ReturnsAsync(mealMembers);

            // Act
            var result = await _controller.GetAllMealMembers();

            // Assert
            if (repositoryHasData)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var data = Assert.IsAssignableFrom<List<GetAllMealMemberResonseDTO>>(okResult.Value);
                Assert.NotNull(data);
                Assert.NotEmpty(data);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetAllMealMembers_Unauthenticated_ReturnsUnauthorized()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };
            // Act
            var result = await _controller.GetAllMealMembers();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetAllMealMembers_InvalidMemberId_ReturnsBadRequest()
        {
            // Arrange
            SetUserClaim("invalid-id");

            // Act
            var result = await _controller.GetAllMealMembers();

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        private void SetUserClaim(string memberId)
        {
            var claims = new List<Claim> { new Claim("Id", memberId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
    }
}