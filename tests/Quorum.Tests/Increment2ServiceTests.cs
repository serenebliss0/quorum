
using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.Tests.Fakes;

namespace Quorum.Tests;

public class Increment2ServiceTests
{
    private readonly InMemoryDataStore _store = new();
    private readonly PolicyService _policyService;
    private readonly CourseService _courseService;
    private readonly RegistrationService _registrationService;

    private readonly User _teacher;
    private readonly User _admin;
    private readonly Student _student;

    public Increment2ServiceTests()
    {
        _policyService = new PolicyService(_store);
        _courseService = new CourseService(_store);
        _registrationService = new RegistrationService(
            _store,
            _policyService
        );

        _teacher = new User
        {
            Id = "teacher-1",
            FullName = "Test Teacher",
            Username = "teacher",
            Role = UserRole.Teacher
        };

        _admin = new User
        {
            Id = "admin-1",
            FullName = "Test Admin",
            Username = "admin",
            Role = UserRole.Administrator
        };

        _student = new Student
        {
            Id = "student-1",
            FullName = "Test Student",
            Username = "student",
            MatricNumber = "TEST001"
        };

        _store.SaveUsers([_teacher, _admin, _student]);

        _store.SavePolicy(new AcademicPolicy
        {
            MinCredits = 12,
            MaxCredits = 24,
            RegistrationStart = DateTime.Now.AddDays(-1),
            RegistrationEnd = DateTime.Now.AddDays(7)
        });
    }

    private Course AddCourse(
        string code = "CSC201",
        int credits = 3,
        int capacity = 30,
        int minToRun = 5)
    {
        Course course = new()
        {
            Code = code,
            Title = "Test Course",
            Credits = credits,
            Capacity = capacity,
            MinToRun = minToRun,
            TeacherId = _teacher.Id
        };

        List<Course> courses = _store.LoadCourses();
        courses.Add(course);
        _store.SaveCourses(courses);

        return course;
    }

    // CourseService tests

    [Fact]
    public void TeacherCanCreateCourse()
    {
        OperationResult result = _courseService.CreateCourse(
            _teacher, "CSC201", "Computer Science", 30, 5
        );

        Assert.True(
            result.IsSuccess,
            $"Course creation failed: {result.Message}"
        );
        Assert.Single(_store.LoadCourses());
        Assert.Equal(3, _store.LoadCourses()[0].Credits);
    }

    [Fact]
    public void NonTeacherCannotCreateCourse()
    {
        OperationResult result = _courseService.CreateCourse(
            _student, "CSC201", "Computer Science", 30, 5
        );

        Assert.False(result.IsSuccess);
        Assert.Empty(_store.LoadCourses());
    }

    [Fact]
    public void CannotCreateDuplicateCourseCode()
    {
        AddCourse();

        OperationResult result = _courseService.CreateCourse(
            _teacher, "csc201", "Another Course", 20, 3
        );

        Assert.False(result.IsSuccess);
        Assert.Single(_store.LoadCourses());
    }

    [Fact]
    public void AdministratorCanUpdateCourseCredits()
    {
        AddCourse();

        OperationResult result = _courseService.UpdateCourseCredits(
            _admin, "CSC201", 6
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(6, _store.LoadCourses()[0].Credits);
    }

    [Fact]
    public void TeacherCannotUpdateCourseCredits()
    {
        AddCourse();

        OperationResult result = _courseService.UpdateCourseCredits(
            _teacher, "CSC201", 6
        );

        Assert.False(result.IsSuccess);
        Assert.Equal(3, _store.LoadCourses()[0].Credits);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void CourseCreditsMustBeBetweenOneAndSix(int credits)
    {
        AddCourse();

        OperationResult result = _courseService.UpdateCourseCredits(
            _admin, "CSC201", credits
        );

        Assert.False(result.IsSuccess);
        Assert.Equal(3, _store.LoadCourses()[0].Credits);
    }

    [Fact]
    public void CourseStatusIsOpenBelowMinimum()
    {
        AddCourse(minToRun: 5);

        Assert.Equal(
            CourseStatus.Open,
            _courseService.GetCourseStatus("CSC201")
        );
    }

    [Fact]
    public void CourseStatusIsRunningAtMinimum()
    {
        Course course = AddCourse(minToRun: 2);

        _store.SaveRegistrations([
            new Registration
            {
                StudentId = _student.Id,
                CourseCode = course.Code
            },
            new Registration
            {
                StudentId = "student-2",
                CourseCode = course.Code
            }
        ]);

        Assert.Equal(
            CourseStatus.Running,
            _courseService.GetCourseStatus("CSC201")
        );
    }

    [Fact]
    public void FullStatusTakesPrecedence()
    {
        Course course = AddCourse(capacity: 2, minToRun: 2);

        _store.SaveRegistrations([
            new Registration
            {
                StudentId = "student-1",
                CourseCode = course.Code
            },
            new Registration
            {
                StudentId = "student-2",
                CourseCode = course.Code
            }
        ]);

        Assert.Equal(
            CourseStatus.Full,
            _courseService.GetCourseStatus("CSC201")
        );
    }

    // PolicyService tests

    [Fact]
    public void AdministratorCanSaveValidPolicy()
    {
        AcademicPolicy policy = new()
        {
            MinCredits = 15,
            MaxCredits = 27,
            RegistrationStart = DateTime.Now,
            RegistrationEnd = DateTime.Now.AddDays(14)
        };

        OperationResult result = _policyService.SavePolicy(
            _admin, policy
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(15, _policyService.GetPolicy().MinCredits);
    }

    [Fact]
    public void TeacherCannotSavePolicy()
    {
        AcademicPolicy policy = _policyService.GetPolicy();

        OperationResult result = _policyService.SavePolicy(
            _teacher, policy
        );

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void PolicyCannotHaveMaxBelowMin()
    {
        AcademicPolicy policy = new()
        {
            MinCredits = 20,
            MaxCredits = 15,
            RegistrationStart = DateTime.Now,
            RegistrationEnd = DateTime.Now.AddDays(7)
        };

        OperationResult result = _policyService.SavePolicy(
            _admin, policy
        );

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void RegistrationPeriodMustEndAfterStart()
    {
        AcademicPolicy policy = new()
        {
            MinCredits = 10,
            MaxCredits = 20,
            RegistrationStart = DateTime.Now.AddDays(7),
            RegistrationEnd = DateTime.Now
        };

        OperationResult result = _policyService.SavePolicy(
            _admin, policy
        );

        Assert.False(result.IsSuccess);
    }

    // RegistrationService tests

    [Fact]
    public void StudentCanRegisterForCourse()
    {
        AddCourse();

        OperationResult result = _registrationService.Register(
            _student.Id, "CSC201"
        );

        Assert.True(result.IsSuccess);
        Assert.Single(_store.LoadRegistrations());
    }

    [Fact]
    public void StudentCanRegisterEvenWhenBelowMinimumCredits()
    {
        AddCourse(credits: 3);

        OperationResult result = _registrationService.Register(
            _student.Id, "CSC201"
        );

        Assert.True(result.IsSuccess);
        Assert.True(
            _registrationService.IsBelowMinCredits(_student.Id)
        );
    }

    [Fact]
    public void StudentCannotRegisterTwice()
    {
        AddCourse();

        _registrationService.Register(_student.Id, "CSC201");

        OperationResult result = _registrationService.Register(
            _student.Id, "CSC201"
        );

        Assert.False(result.IsSuccess);
        Assert.Single(_store.LoadRegistrations());
    }

    [Fact]
    public void StudentCannotRegisterForFullCourse()
    {
        AddCourse(capacity: 1);

        _store.SaveRegistrations([
            new Registration
            {
                StudentId = "another-student",
                CourseCode = "CSC201"
            }
        ]);

        OperationResult result = _registrationService.Register(
            _student.Id, "CSC201"
        );

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void StudentCannotExceedMaximumCredits()
    {
        AddCourse("CSC201", credits: 15);
        AddCourse("CSC202", credits: 12);

        _registrationService.Register(_student.Id, "CSC201");

        OperationResult result = _registrationService.Register(
            _student.Id, "CSC202"
        );

        Assert.False(result.IsSuccess);
        Assert.Single(_store.LoadRegistrations());
    }

    [Fact]
    public void RegistrationIsBlockedOutsideRegistrationPeriod()
    {
        AddCourse();

        _store.SavePolicy(new AcademicPolicy
        {
            MinCredits = 0,
            MaxCredits = 24,
            RegistrationStart = DateTime.Now.AddDays(-10),
            RegistrationEnd = DateTime.Now.AddDays(-1)
        });

        OperationResult result = _registrationService.Register(
            _student.Id, "CSC201"
        );

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void StudentCanDropRegisteredCourse()
    {
        AddCourse();
        _registrationService.Register(_student.Id, "CSC201");

        OperationResult result = _registrationService.Drop(
            _student.Id, "CSC201"
        );

        Assert.True(result.IsSuccess);
        Assert.Empty(_store.LoadRegistrations());
    }

    [Fact]
    public void TeacherCanViewOwnCourseEnrolments()
    {
        AddCourse();
        _registrationService.Register(_student.Id, "CSC201");

        List<Student> students =
            _registrationService.GetCourseEnrolments(
                _teacher, "CSC201"
            );

        Assert.Single(students);
        Assert.Equal(_student.Id, students[0].Id);
    }

    [Fact]
    public void UnrelatedTeacherCannotViewEnrolments()
    {
        AddCourse();
        _registrationService.Register(_student.Id, "CSC201");

        User otherTeacher = new()
        {
            Id = "teacher-2",
            Role = UserRole.Teacher
        };

        List<Student> students =
            _registrationService.GetCourseEnrolments(
                otherTeacher, "CSC201"
            );

        Assert.Empty(students);
    }

    [Fact]
    public void AdministratorCanViewEnrolments()
    {
        AddCourse();
        _registrationService.Register(_student.Id, "CSC201");

        List<Student> students =
            _registrationService.GetCourseEnrolments(
                _admin, "CSC201"
            );

        Assert.Single(students);
    }
}
