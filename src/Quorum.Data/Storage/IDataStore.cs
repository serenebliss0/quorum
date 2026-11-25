using Quorum.Data.Models;

namespace Quorum.Data.Storage;

public interface IDataStore
{
    List<User> LoadUsers();
    void SaveUsers(List<User> users);

    List<Course> LoadCourses();
    void SaveCourses(List<Course> courses);

    List<Registration> LoadRegistrations();
    void SaveRegistrations(List<Registration> registrations);

    AcademicPolicy LoadPolicy();
    void SavePolicy(AcademicPolicy policy);
}