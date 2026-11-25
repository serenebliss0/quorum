namespace Quorum.Data.Models;

public class Course
{
    public string Code { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Credits { get; set; } = 3;

    public int Capacity { get; set; }

    public int MinToRun { get; set; }

    public string TeacherId { get; set; } = string.Empty;
}