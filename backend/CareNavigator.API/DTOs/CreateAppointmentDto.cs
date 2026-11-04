namespace CareNavigator.API.DTOs;

public class CreateAppointmentDto
{
    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Reason { get; set; } = string.Empty;
}