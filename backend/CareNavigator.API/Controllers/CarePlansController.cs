using CareNavigator.API.Data;
using CareNavigator.API.DTOs;
using CareNavigator.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareNavigator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarePlansController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CarePlansController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCarePlan(
        CreateCarePlanDto dto)
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

        if (string.IsNullOrWhiteSpace(dto.Goal))
        {
            return BadRequest("Goal is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Activities))
        {
            return BadRequest("Activities are required.");
        }

        if (dto.EndDate <= dto.StartDate)
        {
            return BadRequest(
                "End date must be greater than start date.");
        }

        var carePlan = new CarePlan
        {
            PatientId = dto.PatientId,
            ProviderId = dto.ProviderId,
            Goal = dto.Goal,
            Activities = dto.Activities,
            Notes = dto.Notes,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = "Active"
        };

        _context.CarePlans.Add(carePlan);

        await _context.SaveChangesAsync();

        return Ok("Care plan created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetCarePlans()
    {
        var carePlans =
            await _context.CarePlans
                .Include(x => x.Patient)
                .Include(x => x.Provider)
                .ToListAsync();

        return Ok(carePlans);
    }

    [HttpPut("complete/{id}")]
    public async Task<IActionResult> CompleteCarePlan(
        int id)
    {
        var carePlan =
            await _context.CarePlans.FindAsync(id);

        if (carePlan == null)
        {
            return NotFound("Care plan not found.");
        }

        if (carePlan.Status == "Completed")
        {
            return BadRequest(
                "Care plan is already completed.");
        }

        carePlan.Status = "Completed";

        await _context.SaveChangesAsync();

        return Ok("Care plan completed.");
    }
}