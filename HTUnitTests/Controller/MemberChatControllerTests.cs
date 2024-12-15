using BusinessObject.Dto.MessageChatDetail;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YourAPINamespace.Controllers;

namespace HTUnitTests.Controller
{
    public class MemberChatControllerTests
    {
        private readonly Mock<IChatMemberRepository> _mockChatRepository;
        private readonly MemberChatController _controller;

        public MemberChatControllerTests()
        {
            _mockChatRepository = new Mock<IChatMemberRepository>();
            _controller = new MemberChatController(_mockChatRepository.Object);

            // Setup default user claims
            var claims = new List<Claim>
            {
                new Claim("Id", "1")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        #region CreateChat Tests
        [Fact]
        public async Task CreateChat_ValidRequest_ReturnsOk()
        {
            // Arrange
            _mockChatRepository
                .Setup(repo => repo.CreateChatAsync(1, "Hello"))
                .Returns(Task.CompletedTask);

            var request = new MemberCreateChatRequest { InitialMessage = "Hello" };

            // Act
            var result = await _controller.CreateChat(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateChat_RepositoryThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockChatRepository
                .Setup(repo => repo.CreateChatAsync(1, "Hello"))
                .ThrowsAsync(new Exception("Test exception"));

            var request = new MemberCreateChatRequest { InitialMessage = "Hello" };

            // Act
            var result = await _controller.CreateChat(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        #endregion

        #region SendMessage Tests
        [Fact]
        public async Task SendMessage_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new SendMessageRequestMember
            {
                ChatId = 1,
                MessageContent = "Test message"
            };
            _mockChatRepository
                .Setup(repo => repo.SendMessageMemberAsync(1, request.ChatId, request.MessageContent))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SendMessage(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(200, okResult.StatusCode); // Asserts the status code
        }

        [Fact]
        public async Task SendMessage_RepositoryThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var request = new SendMessageRequestMember
            {
                ChatId = 1,
                MessageContent = "Test message"
            };
            _mockChatRepository
                .Setup(repo => repo.SendMessageMemberAsync(1, request.ChatId, request.MessageContent))
                .ThrowsAsync(new Exception("Send message failed"));

            // Act
            var result = await _controller.SendMessage(request);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(400, okResult.StatusCode); // Asserts the status code
        }
        #endregion

        #region GetMemberChats Tests
        [Fact]
        public async Task GetMemberChats_ValidRequest_ReturnsOkWithChats()
        {
            // Arrange
            var expectedChats = new List<MessageChat>
            {
                new MessageChat
        {
        MessageChatId = 1,
        MessageChatDetails = new List<MessageChatDetail>
        {
            new MessageChatDetail { MessageChatDetailsId = 1 }
        }
    },
    new MessageChat
    {
        MessageChatId = 2,
        MessageChatDetails = new List<MessageChatDetail>
        {
            new MessageChatDetail { MessageChatDetailsId = 2 }
        }
    }
            };
            _mockChatRepository
                .Setup(repo => repo.GetMemberChatsAsync(1))
                .ReturnsAsync(expectedChats);

            // Act
            var result = await _controller.GetMemberChats();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedChats = okResult.Value as List<MessageChat>;
            Assert.Equal(expectedChats, returnedChats);
        }

        [Fact]
        public async Task GetMemberChats_RepositoryThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockChatRepository
                .Setup(repo => repo.GetMemberChatsAsync(1))
                .ThrowsAsync(new Exception("Fetch chats failed"));

            // Act
            var result = await _controller.GetMemberChats();

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(400, okResult.StatusCode); // Asserts the status code
        }
        #endregion

        #region EndChats Tests
        [Fact]
        public async Task EndChats_ValidRequest_ReturnsOk()
        {
            // Arrange
            _mockChatRepository
                .Setup(repo => repo.EndChatsAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.EndChats();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task EndChats_RepositoryReturnsFalse_ReturnsBadRequest()
        {
            // Arrange
            _mockChatRepository
                .Setup(repo => repo.EndChatsAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.EndChats();

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        #endregion

        #region GetChatDetails Tests
        [Fact]
        public async Task GetChatDetails_ValidRequest_ReturnsOkWithChatDetails()
        {
            // Arrange
            int chatId = 1;
            var expectedChatDetails = new List<GetMessageChatDetailDTO>
            {
                new GetMessageChatDetailDTO { MessageChatDetailsId = chatId }
            };
            _mockChatRepository
                .Setup(repo => repo.GetMemberChatDetailsAsync(1, chatId))
                .ReturnsAsync(expectedChatDetails);

            // Act
            var result = await _controller.GetChatDetails(chatId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedChatDetails = okResult.Value as List<GetMessageChatDetailDTO>;
            Assert.Equal(expectedChatDetails, returnedChatDetails);
        }

        [Fact]
        public async Task GetChatDetails_NullResult_ReturnsUnauthorized()
        {
            // Arrange
            int chatId = 1;
            _mockChatRepository
                .Setup(repo => repo.GetMemberChatDetailsAsync(1, chatId))
                .ReturnsAsync((List<GetMessageChatDetailDTO>)null);

            // Act
            var result = await _controller.GetChatDetails(chatId);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
        #endregion

        #region RateChatInteraction Tests
        [Fact]
        public async Task RateChatInteraction_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new MemberChatRatingRequest
            {
                ChatId = 1,
                RatingStar = 4
            };
            _mockChatRepository
                .Setup(repo => repo.RateChatAsync(1, request.ChatId, request.RatingStar))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RateChatInteraction(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(200, okResult.StatusCode); // Asserts the status code
        }

        [Fact]
        public async Task RateChatInteraction_RepositoryThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var request = new MemberChatRatingRequest
            {
                ChatId = 1,
                RatingStar = 4
            };
            _mockChatRepository
                .Setup(repo => repo.RateChatAsync(1, request.ChatId, request.RatingStar))
                .ThrowsAsync(new Exception("Rating failed"));

            // Act
            var result = await _controller.RateChatInteraction(request);

            // Assert
            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result); // Asserts that the result is 200 OK
            Assert.Equal(400, okResult.StatusCode); // Asserts the status code
        }
        #endregion
    }
}
