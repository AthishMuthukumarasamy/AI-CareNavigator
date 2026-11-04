namespace CareNavigator.API.DTOs;

public class CreatePrescriptionDto
{
    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public string MedicationName { get; set; } = string.Empty;

    public string Dosage { get; set; } = string.Empty;

    public string Instructions { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}