namespace CareNavigator.API.Models;

public class Patient
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string BloodGroup { get; set; } = string.Empty;

    public double Height { get; set; }

    public double Weight { get; set; }

    public string Allergies { get; set; } = string.Empty;

    public string Conditions { get; set; } = string.Empty;

    public string EmergencyContact { get; set; } = string.Empty;

    public User? User { get; set; }
}