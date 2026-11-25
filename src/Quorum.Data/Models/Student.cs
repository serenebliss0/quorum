namespace Quorum.Data.Models;

public class Student : User
{
    public string MatricNumber { get; set; } = string.Empty;

    public Student()
    {
        Role = UserRole.Student;
    }
}