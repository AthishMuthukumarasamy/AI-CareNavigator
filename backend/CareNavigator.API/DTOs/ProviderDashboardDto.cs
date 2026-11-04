namespace CareNavigator.API.DTOs;

public class ProviderDashboardDto
{
    public int TotalPatients { get; set; }

    public int HighRiskPatients { get; set; }

    public int PatientsNeedingFollowUp { get; set; }

    public int ActiveCarePlans { get; set; }

    public int MedicalReportsUploaded { get; set; }

    public string AIInsight { get; set; } = string.Empty;
}