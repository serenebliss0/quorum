using System.IO;
using System.Text.Json;
using Quorum.Data.Models;
using Quorum.Data.Storage;
using Quorum.Logic.Services;

namespace Quorum.UI.Services;

public sealed record CourseDisplayItem(
    string Code,
    string Title,
    int Credits,
    string CapacityText,
    int Capacity,
    int MinToRun,
    string Status);

public sealed record RegistrationDisplayItem(
    string CourseCode,
    string Title,
    int Credits,
    string RegisteredAtText);

public sealed record EnrolmentDisplayItem(
    string MatricNumber,
    string FullName,
    string Username,
    string CourseCode);

public sealed record TeacherDisplayItem(
    string FullName,
    string Username);

/// <summary>
/// UI-facing gateway matching Class Diagram B service contracts plus institution/admin onboarding.
/// AuthService and Admin/Teacher user provisioning run live against IDataStore;
/// CourseService, RegistrationService, and PolicyService calls bridge to Increment 2 Logic once implemented.
/// </summary>
public interface IQuorumGateway
{
    bool HasAdminAccount();
    string GetInstitutionName();
    OperationResult SaveInstitutionName(User actor, string institutionName);
    OperationResult SetupInstitutionAndAdmin(string institutionName, string fullName, string username, string password);

    User? Login(string username, string password);
    OperationResult RegisterStudent(string fullName, string username, string password, string matricNo);
    OperationResult RegisterTeacher(string fullName, string username, string password);

    List<TeacherDisplayItem> GetTeachers(User actor);
    OperationResult AddTeacherByAdmin(User actor, string fullName, string username, string password);

    List<CourseDisplayItem> BrowseCourses();
    List<RegistrationDisplayItem> GetStudentRegistrations(string studentId);
    bool IsBelowMinCredits(string studentId);
    OperationResult Register(string studentId, string courseCode);
    OperationResult Drop(string studentId, string courseCode);

    List<CourseDisplayItem> GetTeacherCourses(User actor);
    OperationResult CreateCourse(User actor, string code, string title, int capacity, int minToRun);
    List<EnrolmentDisplayItem> GetCourseEnrolments(User actor, string courseCode);

    OperationResult UpdateCourseCredits(User actor, string courseCode, int credits);
    AcademicPolicy GetPolicy();
    OperationResult SavePolicy(User actor, AcademicPolicy policy);
}

public sealed class QuorumGateway : IQuorumGateway
{
    private readonly IDataStore _store;
    private readonly AuthService _authService;
    private readonly string _institutionPath;

    private readonly List<CourseDisplayItem> _previewCourses =
    [
        new("SEN201", "Software Engineering", 3, "18 / 40", 40, 10, "Running"),
        new("CSC201", "Data Structures", 3, "40 / 40", 40, 10, "Full"),
        new("MTH201", "Linear Algebra", 3, "2 / 35", 35, 8, "Open"),
    ];

    private readonly List<RegistrationDisplayItem> _previewRegistrations =
    [
        new("SEN201", "Software Engineering", 3, "2026-11-26"),
        new("CSC202", "Operating Systems", 3, "2026-11-26"),
        new("GST201", "Academic Writing", 3, "2026-11-26"),
    ];

    private readonly AcademicPolicy _previewPolicy = new()
    {
        MinCredits = 12,
        MaxCredits = 24,
        RegistrationStart = new DateTime(2026, 11, 1),
        RegistrationEnd = new DateTime(2026, 12, 4),
    };

    public QuorumGateway()
    {
        _store = new JsonDataStore();
        _authService = new AuthService(_store);

        string dataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Quorum");
        Directory.CreateDirectory(dataDir);
        _institutionPath = Path.Combine(dataDir, "institution.json");
    }

    public bool HasAdminAccount()
        => _store.LoadUsers().Any(u => u.Role == UserRole.Administrator);

    public string GetInstitutionName()
    {
        if (!File.Exists(_institutionPath))
            return "Quorum University";

        try
        {
            string json = File.ReadAllText(_institutionPath);
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("InstitutionName", out JsonElement prop))
            {
                string? name = prop.GetString();
                if (!string.IsNullOrWhiteSpace(name))
                    return name;
            }
        }
        catch
        {
            // Fallback to default display name if settings file is unreadable.
        }

        return "Quorum University";
    }

    public OperationResult SaveInstitutionName(User actor, string institutionName)
    {
        if (actor.Role != UserRole.Administrator)
            return OperationResult.Failure("Only an Administrator can update institution settings.");

        if (string.IsNullOrWhiteSpace(institutionName))
            return OperationResult.Failure("Institution name is required.");

        WriteInstitutionName(institutionName.Trim());
        return OperationResult.Success($"Institution updated to {institutionName.Trim()}.");
    }

    public OperationResult SetupInstitutionAndAdmin(
        string institutionName,
        string fullName,
        string username,
        string password)
    {
        if (string.IsNullOrWhiteSpace(institutionName) ||
            string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return OperationResult.Failure("All setup fields are required.");
        }

        List<User> users = _store.LoadUsers();
        if (users.Any(u => u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)))
            return OperationResult.Failure("Username already exists.");

        var (hash, salt) = PasswordHasher.HashPassword(password);
        User admin = new()
        {
            FullName = fullName.Trim(),
            Username = username.Trim(),
            PasswordHash = hash,
            Salt = salt,
            Role = UserRole.Administrator,
        };

        users.Add(admin);
        _store.SaveUsers(users);
        WriteInstitutionName(institutionName.Trim());

        return OperationResult.Success("Institution and Administrator account configured.");
    }

    private void WriteInstitutionName(string name)
    {
        string json = JsonSerializer.Serialize(new { InstitutionName = name });
        File.WriteAllText(_institutionPath, json);
    }

    public User? Login(string username, string password)
        => _authService.Login(username, password);

    public OperationResult RegisterStudent(string fullName, string username, string password, string matricNo)
        => _authService.RegisterStudent(fullName, username, password, matricNo);

    public OperationResult RegisterTeacher(string fullName, string username, string password)
        => _authService.RegisterTeacher(fullName, username, password);

    public List<TeacherDisplayItem> GetTeachers(User actor)
    {
        if (actor.Role != UserRole.Administrator)
            return [];

        return _store.LoadUsers()
            .Where(u => u.Role == UserRole.Teacher)
            .Select(u => new TeacherDisplayItem(u.FullName, u.Username))
            .ToList();
    }

    public OperationResult AddTeacherByAdmin(User actor, string fullName, string username, string password)
    {
        if (actor.Role != UserRole.Administrator)
            return OperationResult.Failure("Only an Administrator can add teachers.");

        return _authService.RegisterTeacher(fullName, username, password);
    }

    public List<CourseDisplayItem> BrowseCourses()
        => [.. _previewCourses];

    public List<RegistrationDisplayItem> GetStudentRegistrations(string studentId)
        => [.. _previewRegistrations];

    public bool IsBelowMinCredits(string studentId)
        => _previewRegistrations.Sum(r => r.Credits) < _previewPolicy.MinCredits;

    public OperationResult Register(string studentId, string courseCode)
        => OperationResult.Failure("RegistrationService pending Increment 2 wiring.");

    public OperationResult Drop(string studentId, string courseCode)
        => OperationResult.Failure("RegistrationService pending Increment 2 wiring.");

    public List<CourseDisplayItem> GetTeacherCourses(User actor)
        =>
        [
            new("SEN201", "Software Engineering", 3, "18 / 40", 40, 10, "Running"),
            new("SEN204", "Formal Methods", 3, "4 / 30", 30, 8, "Open"),
        ];

    public OperationResult CreateCourse(User actor, string code, string title, int capacity, int minToRun)
        => OperationResult.Failure("CourseService pending Increment 2 wiring.");

    public List<EnrolmentDisplayItem> GetCourseEnrolments(User actor, string courseCode)
        =>
        [
            new("24/0102", "AJAYI Oluwasemire", "semire.ajayi", courseCode),
            new("24/0118", "OKEKE Kenechukwu", "kene.okeke", courseCode),
            new("24/0145", "OKIKE Tochukwu", "tochu.okike", courseCode),
        ];

    public OperationResult UpdateCourseCredits(User actor, string courseCode, int credits)
        => OperationResult.Failure("CourseService pending Increment 2 wiring.");

    public AcademicPolicy GetPolicy()
        => _previewPolicy;

    public OperationResult SavePolicy(User actor, AcademicPolicy policy)
        => OperationResult.Failure("PolicyService pending Increment 2 wiring.");
}
