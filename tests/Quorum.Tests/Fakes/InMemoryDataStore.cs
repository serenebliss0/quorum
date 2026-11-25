using Quorum.Data.Models;
using Quorum.Data.Storage;

namespace Quorum.Tests.Fakes;

public class InMemoryDataStore : IDataStore
{
    public List<User> Users { get; private set; } = [];

    public List<Course> Courses { get; private set; } = [];

    public List<Registration> Registrations { get; private set; } = [];

    public AcademicPolicy Policy { get; private set; } = new();

    public List<User> LoadUsers() => Users.ToList();

    public void SaveUsers(List<User> users)
    {
        Users = users.ToList();
    }

    public List<Course> LoadCourses() => Courses.ToList();

    public void SaveCourses(List<Course> courses)
    {
        Courses = courses.ToList();
    }

    public List<Registration> LoadRegistrations() =>
        Registrations.ToList();

    public void SaveRegistrations(List<Registration> registrations)
    {
        Registrations = registrations.ToList();
    }

    public AcademicPolicy LoadPolicy() => Policy;

    public void SavePolicy(AcademicPolicy policy)
    {
        Policy = policy;
    }
}