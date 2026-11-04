namespace CareNavigator.API.DTOs;

public class ChatRequestDto
{
    public int PatientId { get; set; }

    public string Question { get; set; } = string.Empty;
}