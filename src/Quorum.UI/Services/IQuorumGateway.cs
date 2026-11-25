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

/// <summary>
/// UI-facing gateway matching Class Diagram B service contracts.
/// AuthService is wired live; course, registration, and policy calls bridge to
/// Increment 2 Logic services once implemented in Quorum.Logic.
/// </summary>
public interface IQuorumGateway
{
    User? Login(string username, string password);
    OperationResult RegisterStudent(string fullName, string username, string password, string matricNo);
    OperationResult RegisterTeacher(string fullName, string username, string password);

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

/// <summary>
/// Default gateway using real AuthService + JsonDataStore and sample Increment 2 preview data
/// until CourseService, RegistrationService, and PolicyService are added to Quorum.Logic.
/// </summary>
public sealed class QuorumGateway : IQuorumGateway
{
    private readonly AuthService _authService;

    private readonly List<CourseDisplayItem> _previewCourses =
    [
        new("SEN201", "Software Engineering", 3, "18 / 40", 40, 10, "Running"),
        new("CSC201", "Data Structures", 3, "40 / 40", 40, 10, "Full"),
        new("MTH201", "Linear Algebra", 3, "2 / 35", 35, 8, "Open"),
    ];

    private readonly List<RegistrationDisplayItem> _previewRegistrations =
    [
        new("SEN201", "Software Engineering", 3, "2026-11-25"),
        new("CSC202", "Operating Systems", 3, "2026-11-25"),
        new("GST201", "Academic Writing", 3, "2026-11-25"),
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
        IDataStore store = new JsonDataStore();
        _authService = new AuthService(store);
    }

    public User? Login(string username, string password)
        => _authService.Login(username, password);

    public OperationResult RegisterStudent(string fullName, string username, string password, string matricNo)
        => _authService.RegisterStudent(fullName, username, password, matricNo);

    public OperationResult RegisterTeacher(string fullName, string username, string password)
        => _authService.RegisterTeacher(fullName, username, password);

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
