using JobTrackerApi.Data;
using JobTrackerApi.Models;
using JobTrackerApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompanyController(AppDbContext context)
    {
        _context = context;
    }

    // Helper method to get current logged in user's Id from JWT claims
    private Guid GetUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userIdStr);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto dto)
    {
        var userId = GetUserId();

        var company = new Company
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = dto.Name,
            Website = dto.Website,
            Industry = dto.Industry,
            Location = dto.Location,
            CreatedAt = DateTime.UtcNow
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return Ok(new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            Industry = company.Industry,
            Location = company.Location,
            CreatedAt = company.CreatedAt
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var userId = GetUserId();

        var companies = await _context.Companies
            .Where(c => c.UserId == userId)
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Website = c.Website,
                Industry = c.Industry,
                Location = c.Location,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var userId = GetUserId();

        var company = await _context.Companies
            .Where(c => c.Id == id && c.UserId == userId)
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Website = c.Website,
                Industry = c.Industry,
                Location = c.Location,
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (company == null) return NotFound();

        return Ok(company);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        var userId = GetUserId();

        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (company == null)
            return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    


    [HttpPut("{id}")]
public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyUpdateDto dto)
{
    var userId = GetUserId();

    var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

    if (company == null)
        return NotFound();

    company.Name = dto.Name;
    company.Website = dto.Website;
    company.Industry = dto.Industry;
    company.Location = dto.Location;

    await _context.SaveChangesAsync();

    return Ok(new CompanyDto
    {
        Id = company.Id,
        Name = company.Name,
        Website = company.Website,
        Industry = company.Industry,
        Location = company.Location,
        CreatedAt = company.CreatedAt
    });
}


}
