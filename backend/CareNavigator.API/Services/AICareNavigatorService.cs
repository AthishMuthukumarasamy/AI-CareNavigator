using CareNavigator.API.Data;
using Microsoft.EntityFrameworkCore;
using CareNavigator.API.DTOs;

namespace CareNavigator.API.Services;

public class AICareNavigatorService
{
    private readonly ApplicationDbContext _context;

    public AICareNavigatorService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateResponse(int patientId, string question)
    {
        question = question.ToLower();

        if (question.Contains("health status"))
        {
            var appointments = await _context.Appointments
                .CountAsync(x => x.PatientId == patientId);

            var reports = await _context.MedicalReports
                .CountAsync(x => x.PatientId == patientId);

            var prescriptions = await _context.Prescriptions
                .CountAsync(x => x.PatientId == patientId);

            var carePlans = await _context.CarePlans
                .CountAsync(x => x.PatientId == patientId);

            return
            $"""
            Health Summary

            Appointments: {appointments}

            Medical Reports: {reports}

            Prescriptions: {prescriptions}

            Care Plans: {carePlans}

            Recommendation:
            Continue following your healthcare plan.
            """;
        }

        if (question.Contains("medications"))
        {
            var medications =
                await _context.Prescriptions
                    .Where(x => x.PatientId == patientId)
                    .Select(x => x.MedicationName)
                    .ToListAsync();

            if (!medications.Any())
            {
                return "No medications found.";
            }

            return "Current Medications:\n\n" +
                   string.Join("\n", medications);
        }

        if (question.Contains("care plan"))
        {
            var plans =
                await _context.CarePlans
                    .Where(x => x.PatientId == patientId)
                    .Select(x => x.Goal)
                    .ToListAsync();

            if (!plans.Any())
            {
                return "No care plans found.";
            }

            return "Active Care Plans:\n\n" +
                   string.Join("\n", plans);
        }

        return """
            I can help with:

            • Summarize my health status

            • What medications am I taking?

            • Show my care plans
            """;
    }

    public async Task<string> GetFollowUpRecommendation(int patientId)
    {
        var latestAppointment = await _context.Appointments
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(x => x.AppointmentDate)
            .FirstOrDefaultAsync();

        if (latestAppointment == null)
        {
        return """
            No appointments found.

            Recommendation:
            Please schedule your first consultation.
            """;
        }

        var daysSinceVisit =
            (DateTime.Now - latestAppointment.AppointmentDate)
                .TotalDays;

        if (daysSinceVisit > 180)
        {
            return $"""
            Follow-up Recommended

            Last appointment:
            {latestAppointment.AppointmentDate:dd-MMM-yyyy}

            Days Since Visit:
            {(int)daysSinceVisit}

            Recommendation:
            Please schedule a follow-up appointment.
            """;
        }

        return $"""
            Follow-up Status

            Last appointment:
            {latestAppointment.AppointmentDate:dd-MMM-yyyy}

            You are currently up to date.
            """;
    }

    public string GetRiskAssessment(RiskAssessmentDto dto)
    {
        int score = 0;

        var factors = new List<string>();

        if (dto.Age > 50)
        {
            score += 20;
            factors.Add("Age above 50");
        }

        if (dto.Weight > 90)
        {
            score += 20;
            factors.Add("Excess Weight");
        }

        if (dto.BloodPressure.Equals("High", StringComparison.OrdinalIgnoreCase))
        {
            score += 20;
            factors.Add("High Blood Pressure");
        }

        if (dto.BloodSugar.Equals("High",StringComparison.OrdinalIgnoreCase))
        {
            score += 20;
            factors.Add("High Blood Sugar");
        }

        if (dto.Smoker)
        {
            score += 20;
            factors.Add("Smoking");
        }

        string riskLevel = score switch
        {
            >= 80 => "High Risk",
            >= 40 => "Moderate Risk",
            _ => "Low Risk"
        };

        return
            $"""
            Health Risk Assessment

            Risk Score: {score}/100

            Risk Level: {riskLevel}

            Contributing Factors:
            {string.Join(Environment.NewLine, factors.Select(x => $"• {x}"))}

            Recommendation:
            Consult your provider for detailed evaluation.
            """;
    }

    public async Task<string> GetPatientRiskAssessment(
    int patientId)
    {
    int riskScore = 0;

    List<string> factors = new();

    var reports = await _context.MedicalReports
        .Where(x => x.PatientId == patientId)
        .ToListAsync();

    var prescriptions = await _context.Prescriptions
        .Where(x => x.PatientId == patientId)
        .ToListAsync();

    var appointments = await _context.Appointments
        .Where(x => x.PatientId == patientId)
        .ToListAsync();

    foreach (var report in reports)
    {
        var text = report.ExtractedText.ToLower();

        if (text.Contains("diabetes"))
        {
            riskScore += 25;
            factors.Add("Diabetes detected");
        }

        if (text.Contains("cholesterol"))
        {
            riskScore += 20;
            factors.Add("High cholesterol indicators");
        }

        if (text.Contains("blood sugar"))
        {
            riskScore += 20;
            factors.Add("Elevated blood sugar indicators");
        }
    }

    foreach (var prescription in prescriptions)
    {
        if (prescription.MedicationName.Contains(
            "metformin",
            StringComparison.OrdinalIgnoreCase))
        {
            riskScore += 15;
            factors.Add("Diabetes medication prescribed");
        }
    }

    if (!appointments.Any())
    {
        riskScore += 15;
        factors.Add("No appointments recorded");
    }

    riskScore = Math.Min(riskScore, 100);

    string level = riskScore switch
    {
        >= 80 => "High Risk",
        >= 40 => "Moderate Risk",
        _ => "Low Risk"
    };

    return $"""
            Health Risk Assessment

            Risk Score: {riskScore}/100

            Risk Level: {level}

            Contributing Factors:
            {string.Join(Environment.NewLine, factors.Distinct().Select(x => $"• {x}"))}

            Recommendation:
            Review reports and schedule provider consultation if needed.
            """;
    }

    public async Task<List<TimelineEventDto>>
    GetHealthTimeline(int patientId)
    {
        var timeline = new List<TimelineEventDto>();

        var appointments = await _context.Appointments
            .Where(x => x.PatientId == patientId)
            .ToListAsync();

        foreach (var appointment in appointments)
        {
            timeline.Add(new TimelineEventDto
            {
                Date = appointment.CreatedDate,
                EventType = "Appointment",
                Description =
                    $"Appointment {appointment.Status}"
            });
        }

        var reports = await _context.MedicalReports
            .Where(x => x.PatientId == patientId)
            .ToListAsync();

        foreach (var report in reports)
        {
            timeline.Add(new TimelineEventDto
            {
                Date = report.UploadDate,
                EventType = "Medical Report",
                Description =
                    $"{report.ReportName} uploaded"
            });
        }

            var prescriptions =
                await _context.Prescriptions
                    .Where(x => x.PatientId == patientId)
                    .ToListAsync();

        foreach (var prescription in prescriptions)
        {
            timeline.Add(new TimelineEventDto
            {
                Date = prescription.CreatedDate,
                EventType = "Prescription",
                Description =
                    $"{prescription.MedicationName} prescribed"
            });
        }
    

        var carePlans =
            await _context.CarePlans
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

        foreach (var carePlan in carePlans)
        {
            timeline.Add(new TimelineEventDto
            {
                Date = carePlan.CreatedDate,
                EventType = "Care Plan",
                Description = carePlan.Goal
            });
        }
        return timeline
            .OrderByDescending(x => x.Date)
            .ToList();

    }

    public async Task<ProviderDashboardDto>
    GetProviderDashboard()
    {
        var totalPatients =
            await _context.Patients.CountAsync();

        var activeCarePlans =
            await _context.CarePlans
                .CountAsync(x => x.Status == "Active");

        var reportsUploaded =
            await _context.MedicalReports
                .CountAsync();

        int followUpPatients = 0;
        int highRiskPatients = 0;

        var patients = await _context.Patients.ToListAsync();

        foreach (var patient in patients)
        {
            var latestAppointment =
                await _context.Appointments
                    .Where(x => x.PatientId == patient.Id)
                    .OrderByDescending(x => x.AppointmentDate)
                    .FirstOrDefaultAsync();

        if (latestAppointment == null)
        {
            followUpPatients++;
        }
        else
        {
            var days =
                (DateTime.Now -
                 latestAppointment.AppointmentDate)
                .TotalDays;

            if (days > 180)
            {
                followUpPatients++;
            }
        }

        var reports =
            await _context.MedicalReports
                .Where(x => x.PatientId == patient.Id)
                .ToListAsync();

        bool highRisk = reports.Any(r =>
            r.ExtractedText.Contains(
                "diabetes",
                StringComparison.OrdinalIgnoreCase) ||

            r.ExtractedText.Contains(
                "cholesterol",
                StringComparison.OrdinalIgnoreCase) ||

            r.ExtractedText.Contains(
                "blood sugar",
                StringComparison.OrdinalIgnoreCase));

        if (highRisk)
        {
            highRiskPatients++;
        }
        }

    return new ProviderDashboardDto
    {
        TotalPatients = totalPatients,
        HighRiskPatients = highRiskPatients,
        PatientsNeedingFollowUp = followUpPatients,
        ActiveCarePlans = activeCarePlans,
        MedicalReportsUploaded = reportsUploaded,
        AIInsight =
            $"{highRiskPatients} high-risk patients and " +
            $"{followUpPatients} patients require follow-up."
    };
}
}