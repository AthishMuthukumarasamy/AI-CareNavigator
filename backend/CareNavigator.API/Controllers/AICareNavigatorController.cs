using CareNavigator.API.DTOs;
using CareNavigator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareNavigator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AICareNavigatorController : ControllerBase
{
    private readonly AICareNavigatorService _aiService;

    public AICareNavigatorController(
        AICareNavigatorService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat(ChatRequestDto dto)
    {
        var response =
            await _aiService.GenerateResponse(
                dto.PatientId,
                dto.Question);

        return Ok(new
        {
            AIResponse = response
        });
    }

    [HttpGet("follow-up/{patientId}")]
    public async Task<IActionResult> FollowUpRecommendation(int patientId)
    {
        var result =await _aiService
                .GetFollowUpRecommendation(patientId);

        return Ok(new{
        AIResponse = result
        });
    }

    [HttpGet("risk-assessment/{patientId}")]
    public async Task<IActionResult> RiskAssessment(int patientId)
    {
        var result =
            await _aiService
                .GetPatientRiskAssessment(patientId);

        return Ok(new
            {
                AIResponse = result
            });
    }

    [HttpGet("health-timeline/{patientId}")]
    public async Task<IActionResult>
        HealthTimeline(int patientId)
        {
            var timeline =
            await _aiService
                .GetHealthTimeline(patientId);

            return Ok(timeline);
        }

    [HttpGet("provider-dashboard")]
    public async Task<IActionResult>
    ProviderDashboard()
    {
        var dashboard = await _aiService.GetProviderDashboard();
        return Ok(dashboard);
    }
}