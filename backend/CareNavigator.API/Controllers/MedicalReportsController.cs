using CareNavigator.API.Data;
using CareNavigator.API.DTOs;
using CareNavigator.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UglyToad.PdfPig;
using CareNavigator.API.Services;

namespace CareNavigator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly AIReportAnalyzerService _aiService;

    public MedicalReportsController(
        ApplicationDbContext context,
        AIReportAnalyzerService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport(
        CreateMedicalReportDto dto)
    {
        var patient =
            await _context.Patients
                .FindAsync(dto.PatientId);

        if (patient == null)
        {
            return BadRequest("Patient not found.");
        }

        var report = new MedicalReport
        {
            PatientId = dto.PatientId,
            ReportName = dto.ReportName,
            ReportType = dto.ReportType,
            FilePath = dto.FilePath
        };

        _context.MedicalReports.Add(report);

        await _context.SaveChangesAsync();

        return Ok("Medical report created.");
    }

    [HttpGet]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _context.MedicalReports
            .Include(x => x.Patient)
            .ToListAsync();

        return Ok(reports);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadReport(
    [FromForm] UploadMedicalReportDto dto)
    {
        var patient = await _context.Patients.FindAsync(dto.PatientId);

        if (patient == null)
        {
            return BadRequest("Patient not found.");
        }

        if (dto.File == null || dto.File.Length == 0)
        {
            return BadRequest("Please upload a file.");
        }

        var allowedExtensions = new[] { ".pdf" };

        var extension = Path.GetExtension(dto.File.FileName)
        .ToLower();

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Only PDF files are allowed.");
        }

        if (dto.File.Length > 10 * 1024 * 1024)
        {
            return BadRequest("File size cannot exceed 10 MB.");
        }

        var uploadsFolder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName =
            $"{Guid.NewGuid()}_{dto.File.FileName}";

        var filePath =
            Path.Combine(
                uploadsFolder,
                fileName);

        using (var stream =new FileStream(filePath, FileMode.Create)){
            await dto.File.CopyToAsync(stream);
        }

        var report = new MedicalReport
        {
            PatientId = dto.PatientId,
            ReportName = dto.ReportName,
            ReportType = dto.ReportType,
            FilePath = filePath
        };

        _context.MedicalReports.Add(report);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Report uploaded successfully",
            FileName = fileName
        });
    }

    [HttpGet("extract-text/{id}")]
    public async Task<IActionResult> ExtractText(int id)
    {
        var report =
        await _context.MedicalReports
            .FirstOrDefaultAsync(x => x.Id == id);

    if (report == null)
    {
        return NotFound("Medical report not found.");
    }

    if (!System.IO.File.Exists(report.FilePath))
    {
        return BadRequest("File not found.");
    }

    string extractedText = string.Empty;

    using (var document =
           PdfDocument.Open(report.FilePath))
    {
        foreach (var page in document.GetPages())
        {
            extractedText += page.Text;
            extractedText += Environment.NewLine;
        }
    }

    report.ExtractedText = extractedText;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        ReportId = report.Id,
        ReportName = report.ReportName,
        ExtractedText = extractedText
    });
    }
    [HttpPost("generate-summary/{id}")]
    public async Task<IActionResult> GenerateSummary(int id)
    {
        var report = await _context.MedicalReports
            .FindAsync(id);

    if (report == null)
    {
        return NotFound(
            "Medical report not found.");
    }

    if (string.IsNullOrWhiteSpace(
        report.ExtractedText))
    {
        return BadRequest(
            "Please extract text first.");
    }

    var summary =
        _aiService.GenerateSummary(
            report.ExtractedText);

    report.Summary = summary;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        ReportId = report.Id,
        ReportName = report.ReportName,
        Summary = summary
    });
    }
}