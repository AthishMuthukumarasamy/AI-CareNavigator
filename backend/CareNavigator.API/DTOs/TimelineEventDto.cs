namespace CareNavigator.API.DTOs;

public class TimelineEventDto
{
    public DateTime Date { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}