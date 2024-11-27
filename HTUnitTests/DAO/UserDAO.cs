using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using BusinessObject.Models;

public class UserDAOTests : IDisposable
{
    private readonly HealthTrackingDBContext _context;
    private readonly Member _testMember;
    private readonly IServiceProvider _serviceProvider;

    public UserDAOTests()
    {
        // Setup service collection
        var services = new ServiceCollection();

        // Add DbContext using in-memory database
        services.AddDbContext<HealthTrackingDBContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()),
            ServiceLifetime.Transient);

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<HealthTrackingDBContext>();

        // Create test member with hashed password
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash("testPassword", out passwordHash, out passwordSalt);

        _testMember = new Member
        {
            PhoneNumber = "0123456789",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        // Setup mock data
        _context.Members.Add(_testMember);
        _context.SaveChanges();
    }

    // Test for Singleton pattern
    [Fact]
    public void Instance_GetTwice_ReturnsSameInstance()
    {
        // Act
        var instance1 = UserDAO.Instance;
        var instance2 = UserDAO.Instance;

        // Assert
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsMember()
    {
        // Arrange
        var loginRequest = new Member { PhoneNumber = "0123456789" };

        // Override the DBContext in UserDAO for testing
        UserDAO.Instance.SetDbContextForTesting(_context);

        // Act
        var result = await UserDAO.Instance.Login(loginRequest, "testPassword");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testMember.PhoneNumber, result.PhoneNumber);
    }

    [Fact]
    public async Task Login_InvalidPhoneNumber_ReturnsNull()
    {
        // Arrange
        var loginRequest = new Member { PhoneNumber = "9876543210" };

        // Override the DBContext in UserDAO for testing
        UserDAO.Instance.SetDbContextForTesting(_context);

        // Act
        var result = await UserDAO.Instance.Login(loginRequest, "testPassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var loginRequest = new Member { PhoneNumber = "0123456789" };

        // Override the DBContext in UserDAO for testing
        UserDAO.Instance.SetDbContextForTesting(_context);

        // Act
        var result = await UserDAO.Instance.Login(loginRequest, "wrongPassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Login_DatabaseError_ThrowsException()
    {
        // Arrange
        var loginRequest = new Member { PhoneNumber = "0123456789" };

        // Force context to be in an invalid state
        _context.Dispose();

        // Override the DBContext in UserDAO for testing
        UserDAO.Instance.SetDbContextForTesting(_context);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            UserDAO.Instance.Login(loginRequest, "testPassword"));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        (_serviceProvider as IDisposable)?.Dispose();
    }
}

// Modified UserDAO to support testing
public class UserDAO
{
    private static UserDAO instance = null;
    private static readonly object instanceLock = new object();
    private HealthTrackingDBContext _context;

    private UserDAO()
    {
        _context = new HealthTrackingDBContext();
    }

    public static UserDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new UserDAO();
                }
                return instance;
            }
        }
    }

    // Method for testing purposes
    internal void SetDbContextForTesting(HealthTrackingDBContext context)
    {
        _context = context;
    }

    public async Task<Member> Login(Member loginRequestDTO, string password)
    {
        try
        {
            var user = await _context.Members
                .FirstOrDefaultAsync(x => x.PhoneNumber == loginRequestDTO.PhoneNumber);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}