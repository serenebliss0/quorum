using Quorum.Data.Models;
using Quorum.Data.Storage;

namespace Quorum.Logic.Services;

public class AuthService
{
    private readonly IDataStore _store;

    public AuthService(IDataStore store)
    {
        _store = store;
    }

    public OperationResult RegisterStudent(
        string fullName,
        string username,
        string password,
        string matricNo)
    {
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(matricNo))
        {
            return OperationResult.Failure(
                "All fields are required."
            );
        }

        List<User> users = _store.LoadUsers();

        if (users.Any(u =>
                u.Username.Equals(
                    username.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
        {
            return OperationResult.Failure(
                "Username already exists."
            );
        }

        if (users.OfType<Student>().Any(s =>
                s.MatricNumber.Equals(
                    matricNo.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
        {
            return OperationResult.Failure(
                "Matriculation number already exists."
            );
        }

        var credentials = PasswordHasher.HashPassword(password);

        Student student = new()
        {
            FullName = fullName.Trim(),
            Username = username.Trim(),
            MatricNumber = matricNo.Trim(),
            PasswordHash = credentials.Hash,
            Salt = credentials.Salt
        };

        users.Add(student);

        _store.SaveUsers(users);

        return OperationResult.Success(
            "Student account created successfully."
        );
    }

    public OperationResult RegisterTeacher(
        string fullName,
        string username,
        string password)
    {
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return OperationResult.Failure(
                "All fields are required."
            );
        }

        List<User> users = _store.LoadUsers();

        if (users.Any(u =>
                u.Username.Equals(
                    username.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
        {
            return OperationResult.Failure(
                "Username already exists."
            );
        }

        var credentials = PasswordHasher.HashPassword(password);

        User teacher = new()
        {
            FullName = fullName.Trim(),
            Username = username.Trim(),
            PasswordHash = credentials.Hash,
            Salt = credentials.Salt,
            Role = UserRole.Teacher
        };

        users.Add(teacher);

        _store.SaveUsers(users);

        return OperationResult.Success(
            "Teacher account created successfully."
        );
    }

    public User? Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        List<User> users = _store.LoadUsers();

        User? user = users.FirstOrDefault(u =>
            u.Username.Equals(
                username.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (user is null)
            return null;

        bool passwordIsValid = PasswordHasher.VerifyPassword(
            password,
            user.PasswordHash,
            user.Salt
        );

        return passwordIsValid ? user : null;
    }
}