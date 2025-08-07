using JobTrackerApi.Data;
using JobTrackerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ApplicationCreateDto dto)
    {
        // Get the logged-in user's ID from JWT claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        // Check if company exists
        var company = await _context.Companies.FindAsync(dto.CompanyId);
        if (company == null)
            return BadRequest(new { message = "Invalid company." });

        var application = new Application
        {
            UserId = Guid.Parse(userId),
            CompanyId = dto.CompanyId,
            JobTitle = dto.JobTitle,
            JobLocation = dto.JobLocation,
            JobType = dto.JobType,
            JobLink = dto.JobLink,
            ApplicationDate = dto.ApplicationDate,
            Status = dto.Status,
            Deadline = dto.Deadline,
            CreatedAt = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Application created successfully.", applicationId = application.Id });
    }
}
