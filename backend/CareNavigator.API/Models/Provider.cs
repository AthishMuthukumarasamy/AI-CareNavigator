namespace CareNavigator.API.Models;

public class Provider
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Specialization { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public User? User { get; set; }
}