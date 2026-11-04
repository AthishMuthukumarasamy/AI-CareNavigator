namespace CareNavigator.API.DTOs;

public class ApproveAppointmentDto
{
    public int AppointmentId { get; set; }

    public string Notes { get; set; } = string.Empty;
}