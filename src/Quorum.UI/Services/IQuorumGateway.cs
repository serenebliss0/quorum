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
/// UI-facing gateway delegating all operations to Quorum.Logic services and IDataStore.
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
    private readonly PolicyService _policyService;
    private readonly CourseService _courseService;
    private readonly RegistrationService _registrationService;
    private readonly string _institutionPath;

    public QuorumGateway()
    {
        _store = new JsonDataStore();
        _authService = new AuthService(_store);
        _policyService = new PolicyService(_store);
        _courseService = new CourseService(_store);
        _registrationService = new RegistrationService(_store, _policyService);

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

        // Seed a valid default AcademicPolicy on first setup so registrations are usable immediately.
        AcademicPolicy existingPolicy = _policyService.GetPolicy();
        if (existingPolicy.RegistrationEnd <= existingPolicy.RegistrationStart)
        {
            _policyService.SavePolicy(admin, CreateDefaultPolicy());
        }

        return OperationResult.Success("Institution and Administrator account configured.");
    }

    private static AcademicPolicy CreateDefaultPolicy() => new()
    {
        MinCredits = 12,
        MaxCredits = 24,
        RegistrationStart = DateTime.Today,
        RegistrationEnd = DateTime.Today.AddDays(30).AddDays(1).AddSeconds(-1),
    };

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
        => MapCourses(_courseService.BrowseCourses());

    public List<RegistrationDisplayItem> GetStudentRegistrations(string studentId)
    {
        List<Registration> regs = _registrationService.GetStudentRegistrations(studentId);
        List<Course> courses = _courseService.BrowseCourses();

        return regs.Select(r =>
        {
            Course? course = courses.FirstOrDefault(c =>
                c.Code.Equals(r.CourseCode, StringComparison.OrdinalIgnoreCase));
            return new RegistrationDisplayItem(
                r.CourseCode,
                course?.Title ?? r.CourseCode,
                course?.Credits ?? 0,
                r.RegisteredAt.ToString("yyyy-MM-dd HH:mm"));
        }).ToList();
    }

    public bool IsBelowMinCredits(string studentId)
        => _registrationService.IsBelowMinCredits(studentId);

    public OperationResult Register(string studentId, string courseCode)
        => _registrationService.Register(studentId, courseCode);

    public OperationResult Drop(string studentId, string courseCode)
        => _registrationService.Drop(studentId, courseCode);

    public List<CourseDisplayItem> GetTeacherCourses(User actor)
        => MapCourses(_courseService.GetTeacherCourses(actor));

    public OperationResult CreateCourse(User actor, string code, string title, int capacity, int minToRun)
        => _courseService.CreateCourse(actor, code, title, capacity, minToRun);

    public List<EnrolmentDisplayItem> GetCourseEnrolments(User actor, string courseCode)
    {
        return _registrationService.GetCourseEnrolments(actor, courseCode)
            .Select(s => new EnrolmentDisplayItem(s.MatricNumber, s.FullName, s.Username, courseCode))
            .ToList();
    }

    public OperationResult UpdateCourseCredits(User actor, string courseCode, int credits)
        => _courseService.UpdateCourseCredits(actor, courseCode, credits);

    public AcademicPolicy GetPolicy()
    {
        AcademicPolicy policy = _policyService.GetPolicy();
        return policy.RegistrationEnd <= policy.RegistrationStart
            ? CreateDefaultPolicy()
            : policy;
    }

    public OperationResult SavePolicy(User actor, AcademicPolicy policy)
        => _policyService.SavePolicy(actor, policy);

    private List<CourseDisplayItem> MapCourses(List<Course> courses)
    {
        List<Registration> regs = _store.LoadRegistrations();
        return courses.Select(c =>
        {
            int enrolled = regs.Count(r =>
                r.CourseCode.Equals(c.Code, StringComparison.OrdinalIgnoreCase));
            string status = _courseService.GetCourseStatus(c.Code).ToString();
            return new CourseDisplayItem(
                c.Code,
                c.Title,
                c.Credits,
                $"{enrolled} / {c.Capacity}",
                c.Capacity,
                c.MinToRun,
                status);
        }).ToList();
    }
}
