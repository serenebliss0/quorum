
using Quorum.Data.Models;
using Quorum.Data.Storage;

namespace Quorum.Logic.Services;

public class RegistrationService
{
    private readonly IDataStore _store;
    private readonly PolicyService _policyService;

    public RegistrationService(
        IDataStore store,
        PolicyService policyService)
    {
        _store = store;
        _policyService = policyService;
    }

    public OperationResult Register(
        string studentId,
        string courseCode)
    {
        if (string.IsNullOrWhiteSpace(studentId) ||
            string.IsNullOrWhiteSpace(courseCode))
        {
            return OperationResult.Failure(
                "Student ID and course code are required."
            );
        }

        Student? student = _store.LoadUsers()
            .OfType<Student>()
            .FirstOrDefault(s => s.Id == studentId);

        if (student is null)
        {
            return OperationResult.Failure(
                "Student account not found."
            );
        }

        List<Course> courses = _store.LoadCourses();

        Course? course = courses.FirstOrDefault(c =>
            c.Code.Equals(
                courseCode.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (course is null)
        {
            return OperationResult.Failure(
                "Course not found."
            );
        }

        if (!_policyService.IsRegistrationOpen(DateTime.Now))
        {
            return OperationResult.Failure(
                "Course registration is currently closed."
            );
        }

        List<Registration> registrations =
            _store.LoadRegistrations();

        bool alreadyRegistered = registrations.Any(r =>
            r.StudentId == studentId &&
            r.CourseCode.Equals(
                course.Code,
                StringComparison.OrdinalIgnoreCase));

        if (alreadyRegistered)
        {
            return OperationResult.Failure(
                "You are already registered for this course."
            );
        }

        int enrolmentCount = registrations.Count(r =>
            r.CourseCode.Equals(
                course.Code,
                StringComparison.OrdinalIgnoreCase));

        if (enrolmentCount >= course.Capacity)
        {
            return OperationResult.Failure(
                "This course is already full."
            );
        }

        int currentCredits = GetTotalCredits(
            studentId,
            registrations,
            courses
        );

        if (currentCredits + course.Credits >
            _policyService.GetPolicy().MaxCredits)
        {
            return OperationResult.Failure(
                "Registration would exceed the maximum credit limit."
            );
        }

        registrations.Add(new Registration
        {
            StudentId = studentId,
            CourseCode = course.Code,
            RegisteredAt = DateTime.Now
        });

        _store.SaveRegistrations(registrations);

        return OperationResult.Success(
            "Course registration successful."
        );
    }

    public OperationResult Drop(
        string studentId,
        string courseCode)
    {
        if (string.IsNullOrWhiteSpace(studentId) ||
            string.IsNullOrWhiteSpace(courseCode))
        {
            return OperationResult.Failure(
                "Student ID and course code are required."
            );
        }

        Student? student = _store.LoadUsers()
            .OfType<Student>()
            .FirstOrDefault(s => s.Id == studentId);

        if (student is null)
        {
            return OperationResult.Failure(
                "Student account not found."
            );
        }

        List<Registration> registrations =
            _store.LoadRegistrations();

        Registration? registration = registrations.FirstOrDefault(r =>
            r.StudentId == studentId &&
            r.CourseCode.Equals(
                courseCode.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (registration is null)
        {
            return OperationResult.Failure(
                "Registration not found."
            );
        }

        registrations.Remove(registration);
        _store.SaveRegistrations(registrations);

        return OperationResult.Success(
            "Course dropped successfully."
        );
    }

    public List<Registration> GetStudentRegistrations(
        string studentId)
    {
        return _store.LoadRegistrations()
            .Where(r => r.StudentId == studentId)
            .ToList();
    }

    public List<Student> GetCourseEnrolments(
        User actor,
        string courseCode)
    {
        if (actor is null ||
            string.IsNullOrWhiteSpace(courseCode))
        {
            return [];
        }

        Course? course = _store.LoadCourses()
            .FirstOrDefault(c =>
                c.Code.Equals(
                    courseCode.Trim(),
                    StringComparison.OrdinalIgnoreCase));

        if (course is null)
            return [];

        bool isAdministrator =
            actor.Role == UserRole.Administrator;

        bool isCourseTeacher =
            actor.Role == UserRole.Teacher &&
            course.TeacherId == actor.Id;

        if (!isAdministrator && !isCourseTeacher)
            return [];

        List<string> studentIds = _store.LoadRegistrations()
            .Where(r => r.CourseCode.Equals(
                course.Code,
                StringComparison.OrdinalIgnoreCase))
            .Select(r => r.StudentId)
            .Distinct()
            .ToList();

        return _store.LoadUsers()
            .OfType<Student>()
            .Where(s => studentIds.Contains(s.Id))
            .ToList();
    }

    public bool IsBelowMinCredits(string studentId)
    {
        List<Registration> registrations =
            _store.LoadRegistrations();

        int totalCredits = GetTotalCredits(
            studentId,
            registrations,
            _store.LoadCourses()
        );

        return totalCredits < _policyService.GetPolicy().MinCredits;
    }

    private static int GetTotalCredits(
        string studentId,
        List<Registration> registrations,
        List<Course> courses)
    {
        List<string> registeredCourseCodes = registrations
            .Where(r => r.StudentId == studentId)
            .Select(r => r.CourseCode)
            .ToList();

        return courses
            .Where(c => registeredCourseCodes.Contains(
                c.Code,
                StringComparer.OrdinalIgnoreCase))
            .Sum(c => c.Credits);
    }
}
