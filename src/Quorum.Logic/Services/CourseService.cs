
using Quorum.Data.Models;
using Quorum.Data.Storage;

namespace Quorum.Logic.Services;

public class CourseService
{
    private readonly IDataStore _store;

    public CourseService(IDataStore store)
    {
        _store = store;
    }

    public OperationResult CreateCourse(
        User actor,
        string code,
        string title,
        int capacity,
        int minToRun)
    {
        if (actor is null ||
            (actor.Role != UserRole.Teacher &&
            actor.Role != UserRole.Administrator))
        {
            return OperationResult.Failure(
                "Only teachers and admins can create courses."
            ); 
            //TODO update Use Case Diagram. 
            // TODO An admin should be able to create courses as well
        }

        if (string.IsNullOrWhiteSpace(code) ||
            string.IsNullOrWhiteSpace(title))
        {
            return OperationResult.Failure(
                "Course code and title are required."
            );
        }

        if (capacity <= 0 || minToRun < 0 || minToRun > capacity)
        {
            return OperationResult.Failure(
                "Invalid course capacity or minimum-to-run value."
            );
        }

        List<Course> courses = _store.LoadCourses();

        if (courses.Any(c =>
                c.Code.Equals(
                    code.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
        {
            return OperationResult.Failure(
                "A course with this code already exists."
            );
        }

        Course course = new()
        {
            Code = code.Trim(),
            Title = title.Trim(),
            Capacity = capacity,
            MinToRun = minToRun,
            Credits = 3,
            TeacherId = actor.Id
        };

        courses.Add(course);
        _store.SaveCourses(courses);

        return OperationResult.Success(
            "Course created successfully."
        );
    }

    public List<Course> BrowseCourses()
    {
        return _store.LoadCourses();
    }

    public List<Course> GetTeacherCourses(User actor)
    {
        if (actor is null || actor.Role != UserRole.Teacher)
            return [];

        return _store.LoadCourses()
            .Where(c => c.TeacherId == actor.Id)
            .ToList();
    }

    public OperationResult UpdateCourseCredits(
        User actor,
        string courseCode,
        int credits)
    {
        if (actor is null ||
            actor.Role != UserRole.Administrator)
        {
            return OperationResult.Failure(
                "Only administrators can update course credits."
            );
        }

        if (credits < 1 || credits > 6)
        {
            return OperationResult.Failure(
                "Course credits must be between 1 and 6."
            );
        }

        List<Course> courses = _store.LoadCourses();

        Course? course = courses.FirstOrDefault(c =>
            c.Code.Equals(
                courseCode?.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (course is null)
        {
            return OperationResult.Failure(
                "Course not found."
            );
        }

        course.Credits = credits;
        _store.SaveCourses(courses);

        return OperationResult.Success(
            "Course credits updated successfully."
        );
    }

    public CourseStatus GetCourseStatus(string courseCode)
    {
        Course? course = _store.LoadCourses()
            .FirstOrDefault(c =>
                c.Code.Equals(
                    courseCode,
                    StringComparison.OrdinalIgnoreCase));

        if (course is null)
            throw new KeyNotFoundException("Course not found.");

        int enrolmentCount = _store.LoadRegistrations()
            .Count(r => r.CourseCode.Equals(
                course.Code,
                StringComparison.OrdinalIgnoreCase));

        if (enrolmentCount >= course.Capacity)
            return CourseStatus.Full;

        if (enrolmentCount >= course.MinToRun)
            return CourseStatus.Running;

        return CourseStatus.Open;
    }
}
