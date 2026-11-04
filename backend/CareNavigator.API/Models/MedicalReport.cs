namespace CareNavigator.API.Models;

public class MedicalReport
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string ReportName { get; set; } = string.Empty;

    public string ReportType { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ExtractedText { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }
}