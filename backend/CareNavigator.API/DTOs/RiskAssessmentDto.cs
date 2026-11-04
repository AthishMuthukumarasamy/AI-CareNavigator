namespace CareNavigator.API.DTOs;

public class RiskAssessmentDto
{
    public int Age { get; set; }

    public double Weight { get; set; }

    public string BloodPressure { get; set; } = string.Empty;

    public string BloodSugar { get; set; } = string.Empty;

    public bool Smoker { get; set; }
}