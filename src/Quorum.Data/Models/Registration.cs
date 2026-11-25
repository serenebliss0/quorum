namespace Quorum.Data.Models;

public class Registration
{
    public string StudentId { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public DateTime RegisteredAt { get; set; }
}