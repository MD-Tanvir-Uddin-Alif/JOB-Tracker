using JobTrackerApi.Data;
using JobTrackerApi.Models;
using JobTrackerApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobTrackerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/notes
        [HttpPost]
        public async Task<IActionResult> Create(NoteCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // Ensure application belongs to user
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == dto.ApplicationId && a.UserId == Guid.Parse(userId));

            if (application == null)
                return Forbid("You do not have access to this application.");

            var note = new Note
            {
                ApplicationId = dto.ApplicationId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Note added successfully.", noteId = note.Id });
        }

        // GET: api/notes/{applicationId}
        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetNotes(Guid applicationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var applicationExists = await _context.Applications
                .AnyAsync(a => a.Id == applicationId && a.UserId == Guid.Parse(userId));

            if (!applicationExists)
                return Forbid("You do not have access to this application.");

            var notes = await _context.Notes
                .Where(n => n.ApplicationId == applicationId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(notes);
        }

        // DELETE: api/notes/{noteId}
        [HttpDelete("{noteId}")]
        public async Task<IActionResult> Delete(Guid noteId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var note = await _context.Notes
                .Include(n => n.Application)
                .FirstOrDefaultAsync(n => n.Id == noteId && n.Application.UserId == Guid.Parse(userId));

            if (note == null)
                return NotFound(new { message = "Note not found or access denied." });

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Note deleted successfully." });
        }
    }
}
