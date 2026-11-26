using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.Tests.Fakes;

namespace Quorum.Tests;

public class AuthServiceTests
{
    private readonly InMemoryDataStore _store;
    private readonly AuthService _auth;

    public AuthServiceTests()
    {
        _store = new InMemoryDataStore();
        _auth = new AuthService(_store);
    }

    [Fact]
    public void RegisterStudent_ValidDetails_CreatesAccount()
    {
        OperationResult result = _auth.RegisterStudent(
            "Semire Ajayi",
            "semire",
            "password123",
            "PAU123"
        );

        Assert.True(result.IsSuccess);
        Assert.Single(_store.Users);

        Student student = Assert.IsType<Student>(
            _store.Users[0]
        );

        Assert.Equal("PAU123", student.MatricNumber);
        Assert.Equal(UserRole.Student, student.Role);
    }

    [Fact]
    public void RegisterStudent_DuplicateUsername_Fails()
    {
        _auth.RegisterStudent(
            "First Student",
            "student",
            "password123",
            "PAU001"
        );

        OperationResult result = _auth.RegisterStudent(
            "Second Student",
            "STUDENT",
            "password456",
            "PAU002"
        );

        Assert.False(result.IsSuccess);
        Assert.Single(_store.Users);
    }

    [Fact]
    public void RegisterStudent_DuplicateMatricNumber_Fails()
    {
        _auth.RegisterStudent(
            "First Student",
            "student1",
            "password123",
            "PAU001"
        );

        OperationResult result = _auth.RegisterStudent(
            "Second Student",
            "student2",
            "password456",
            "pau001"
        );

        Assert.False(result.IsSuccess);
        Assert.Single(_store.Users);
    }

    [Fact]
    public void RegisterTeacher_ValidDetails_CreatesTeacher()
    {
        OperationResult result = _auth.RegisterTeacher(
            "Teacher One",
            "teacher1",
            "password123"
        );

        Assert.True(result.IsSuccess);
        Assert.Single(_store.Users);
        Assert.Equal(UserRole.Teacher, _store.Users[0].Role);
    }

    [Fact]
    public void RegisterTeacher_DuplicateUsername_Fails()
    {
        _auth.RegisterTeacher(
            "Teacher One",
            "teacher",
            "password123"
        );

        OperationResult result = _auth.RegisterTeacher(
            "Teacher Two",
            "TEACHER",
            "password456"
        );

        Assert.False(result.IsSuccess);
        Assert.Single(_store.Users);
    }

    [Fact]
    public void RegisterStudent_MissingFields_Fails()
    {
        OperationResult result = _auth.RegisterStudent(
            "",
            "student",
            "password123",
            "PAU001"
        );

        Assert.False(result.IsSuccess);
        Assert.Empty(_store.Users);
    }

    [Fact]
    public void Login_ValidCredentials_ReturnsUser()
    {
        _auth.RegisterStudent(
            "Semire Ajayi",
            "semire",
            "password123",
            "PAU123"
        );

        User? user = _auth.Login(
            "semire",
            "password123"
        );

        Assert.NotNull(user);
        Assert.Equal("semire", user.Username);
    }

    [Fact]
    public void Login_CaseInsensitiveUsername_Succeeds()
    {
        _auth.RegisterStudent(
            "Semire Ajayi",
            "semire",
            "password123",
            "PAU123"
        );

        User? user = _auth.Login(
            "SEMIRE",
            "password123"
        );

        Assert.NotNull(user);
    }

    [Fact]
    public void Login_WrongPassword_ReturnsNull()
    {
        _auth.RegisterStudent(
            "Semire Ajayi",
            "semire",
            "password123",
            "PAU123"
        );

        User? user = _auth.Login(
            "semire",
            "wrongpassword"
        );

        Assert.Null(user);
    }

    [Fact]
    public void Login_UnknownUsername_ReturnsNull()
    {
        User? user = _auth.Login(
            "unknown",
            "password123"
        );

        Assert.Null(user);
    }

    [Fact]
    public void PasswordHasher_SamePassword_UsesDifferentSalts()
    {
        var first = PasswordHasher.HashPassword("password123");
        var second = PasswordHasher.HashPassword("password123");

        Assert.NotEqual(first.Salt, second.Salt);
        Assert.NotEqual(first.Hash, second.Hash);

        Assert.True(
            PasswordHasher.VerifyPassword(
                "password123",
                first.Hash,
                first.Salt
            )
        );
    }
}