namespace CareNavigator.API.DTOs;

public class CreateCarePlanDto
{
    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public string Goal { get; set; } = string.Empty;

    public string Activities { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}