namespace Quorum.Data.Models;

public class AcademicPolicy
{
    public int MinCredits { get; set; }

    public int MaxCredits { get; set; }

    public DateTime RegistrationStart { get; set; }

    public DateTime RegistrationEnd { get; set; }
}