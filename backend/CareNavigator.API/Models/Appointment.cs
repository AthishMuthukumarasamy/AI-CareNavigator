namespace CareNavigator.API.Models;

public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }

    public Provider? Provider { get; set; }
}