using CareNavigator.API.Data;
using CareNavigator.API.DTOs;
using CareNavigator.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareNavigator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
    CreateAppointmentDto dto)
    {
    // Validation 1 - Patient must exist
    var patient = await _context.Patients
        .FindAsync(dto.PatientId);

    if (patient == null)
    {
        return BadRequest("Patient not found.");
    }

    // Validation 2 - Provider must exist
    var provider = await _context.Providers
        .FindAsync(dto.ProviderId);

    if (provider == null)
    {
        return BadRequest("Provider not found.");
    }

    // Validation 3 - Appointment date cannot be in the past
    if (dto.AppointmentDate < DateTime.Now)
    {
        return BadRequest(
            "Appointment date cannot be in the past.");
    }

    // Validation 4 - Reason is required
    if (string.IsNullOrWhiteSpace(dto.Reason))
    {
        return BadRequest(
            "Reason is required.");
    }

    // Validation 5 - Provider already has appointment at same time
    var existingAppointment =
        await _context.Appointments
            .AnyAsync(x =>
                x.ProviderId == dto.ProviderId &&
                x.AppointmentDate == dto.AppointmentDate &&
                x.Status != "Cancelled");

    if (existingAppointment)
    {
        return BadRequest(
            "Provider already has an appointment at this time.");
    }

    var appointment = new Appointment
    {
        PatientId = dto.PatientId,
        ProviderId = dto.ProviderId,
        AppointmentDate = dto.AppointmentDate,
        Reason = dto.Reason,
        Status = "Pending"
    };

    _context.Appointments.Add(appointment);

    await _context.SaveChangesAsync();

    return Ok("Appointment created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments()
    {
        var appointments = await _context.Appointments
            .Include(x => x.Patient)
            .Include(x => x.Provider)
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpPut("approve/{id}")]
    public async Task<IActionResult> ApproveAppointment(
        int id)
    {
        var appointment =
            await _context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        if (appointment.Status == "Cancelled")
        {
            return BadRequest("Cancelled appointment cannot be approved.");
        }

        if (appointment.Status == "Approved")
        {
            return BadRequest("Appointment is already approved.");
        }

        appointment.Status = "Approved";

        await _context.SaveChangesAsync();

        return Ok("Appointment approved.");
    }

    [HttpPut("complete/{id}")]
    public async Task<IActionResult> CompleteAppointment(
        int id)
    {
        var appointment =
            await _context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        if (appointment.Status == "Completed")
        {
            return BadRequest("Appointment is already completed.");
        }

        if (appointment.Status != "Approved")
        {
            return BadRequest("Only approved appointments can be completed.");
        
        }

        appointment.Status = "Completed";

        await _context.SaveChangesAsync();

        return Ok("Appointment completed.");
    }

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> CancelAppointment(
        int id)
    {
        var appointment =
            await _context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        if (appointment.Status == "Completed")
        {
            return BadRequest("Completed appointment cannot be cancelled.");
        }
        appointment.Status = "Cancelled";

        await _context.SaveChangesAsync();

        return Ok("Appointment cancelled.");
    }
}