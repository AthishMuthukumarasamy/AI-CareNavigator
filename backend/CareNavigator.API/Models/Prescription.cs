namespace CareNavigator.API.Models;

public class Prescription
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public string MedicationName { get; set; } = string.Empty;

    public string Dosage { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }

    public Provider? Provider { get; set; }
}