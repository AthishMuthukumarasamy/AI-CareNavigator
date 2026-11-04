using BCrypt.Net;
using CareNavigator.API.Data;
using CareNavigator.API.DTOs;
using CareNavigator.API.Models;
using CareNavigator.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareNavigator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(
        ApplicationDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterDto dto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest(
                "Email already exists.");
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt
                .HashPassword(dto.Password),
            Role = dto.Role
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == dto.Email);

        if (user == null)
        {
            return Unauthorized(
                "Invalid email or password.");
        }

        bool validPassword =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return Unauthorized(
                "Invalid email or password.");
        }

        var token =
            _jwtService.GenerateToken(user);

        return Ok(new
        {
            token,
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role
        });
    }
}