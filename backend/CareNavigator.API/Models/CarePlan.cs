namespace CareNavigator.API.Models;

public class CarePlan
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public string Goal { get; set; } = string.Empty;

    public string Activities { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "Active";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }

    public Provider? Provider { get; set; }
}