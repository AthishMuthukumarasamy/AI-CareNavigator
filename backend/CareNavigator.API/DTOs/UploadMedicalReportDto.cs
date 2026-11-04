using Microsoft.AspNetCore.Http;

namespace CareNavigator.API.DTOs;

public class UploadMedicalReportDto
{
    public int PatientId { get; set; }

    public string ReportName { get; set; } = string.Empty;

    public string ReportType { get; set; } = string.Empty;

    public IFormFile? File { get; set; }
}