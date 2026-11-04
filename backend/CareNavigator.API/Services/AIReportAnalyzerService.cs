namespace CareNavigator.API.Services;

public class AIReportAnalyzerService
{
    public string GenerateSummary(string reportText)
    {
        var summary = "AI Medical Summary\n\n";

        if (string.IsNullOrWhiteSpace(reportText))
        {
            return "No report content available for analysis.";
        }

        if (reportText.Contains("diabetes",
            StringComparison.OrdinalIgnoreCase))
        {
            summary += "• Diabetes-related information detected.\n";
        }

        if (reportText.Contains("cholesterol",
            StringComparison.OrdinalIgnoreCase))
        {
            summary += "• Cholesterol information detected.\n";
        }

        if (reportText.Contains("blood sugar",
            StringComparison.OrdinalIgnoreCase))
        {
            summary += "• Blood sugar information detected.\n";
        }

        if (reportText.Contains("vitamin",
            StringComparison.OrdinalIgnoreCase))
        {
            summary += "• Vitamin-related information detected.\n";
        }

        summary += "\nRecommendation:\n";
        summary += "Please consult your healthcare provider.";

        return summary;
    }
}