using CareNavigator.API.Data;
using CareNavigator.API.DTOs;
using CareNavigator.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareNavigator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PrescriptionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription(
        CreatePrescriptionDto dto)
    {
        var patient =
            await _context.Patients.FindAsync(dto.PatientId);

        if (patient == null)
        {
            return BadRequest("Patient not found.");
        }

        var provider =
            await _context.Providers.FindAsync(dto.ProviderId);

        if (provider == null)
        {
            return BadRequest("Provider not found.");
        }

        if (string.IsNullOrWhiteSpace(dto.MedicationName))
        {
            return BadRequest("Medication name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Dosage))
        {
            return BadRequest("Dosage is required.");
        }

        if (dto.EndDate <= dto.StartDate)
        {
            return BadRequest(
                "End date must be greater than start date.");
        }

        var prescription = new Prescription
        {
            PatientId = dto.PatientId,
            ProviderId = dto.ProviderId,
            MedicationName = dto.MedicationName,
            Dosage = dto.Dosage,
            Instructions = dto.Instructions,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        _context.Prescriptions.Add(prescription);

        await _context.SaveChangesAsync();

        return Ok("Prescription created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetPrescriptions()
    {
        var prescriptions =
            await _context.Prescriptions
                .Include(x => x.Patient)
                .Include(x => x.Provider)
                .ToListAsync();

        return Ok(prescriptions);
    }
}