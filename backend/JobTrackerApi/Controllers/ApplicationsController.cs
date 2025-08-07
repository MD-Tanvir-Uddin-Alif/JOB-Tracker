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

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(ApplicationCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

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

    // GET ALL FOR USER
    [HttpGet]
    public async Task<IActionResult> GetUserApplications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var applications = await _context.Applications
            .Include(a => a.Company)
            .Where(a => a.UserId == userGuid)
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync();

        return Ok(applications);
    }

    // UPDATE BY ID
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApplication(Guid id, [FromBody] ApplicationUpdateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userGuid);
        if (application == null)
            return NotFound(new { message = "Application not found or unauthorized." });

        application.JobTitle = dto.JobTitle;
        application.CompanyId = dto.CompanyId;
        application.JobType = dto.JobType;
        application.Status = dto.Status;
        application.JobLink = dto.JobLink;
        application.ApplicationDate = dto.ApplicationDate;
        application.JobLocation = dto.JobLocation;
        application.Deadline = dto.Deadline;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Application updated successfully." });
    }

    // DELETE BY ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApplication(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userGuid);
        if (application == null)
            return NotFound(new { message = "Application not found or unauthorized." });

        _context.Applications.Remove(application);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Application deleted successfully." });
    }
}
