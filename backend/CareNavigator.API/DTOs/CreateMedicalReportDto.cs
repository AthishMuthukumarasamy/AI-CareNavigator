namespace CareNavigator.API.DTOs;

public class CreateMedicalReportDto
{
    public int PatientId { get; set; }

    public string ReportName { get; set; } = string.Empty;

    public string ReportType { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;
}