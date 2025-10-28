using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocietyApi.Data;
using SocietyApi.Models;

namespace SocietyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaintenanceController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MaintenanceController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.MaintenanceRequests.OrderByDescending(m => m.CreatedAt).ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMaintenanceDto dto)
        {
            var username = User.Identity?.Name ?? "anonymous";
            var item = new MaintenanceRequest
            {
                Title = dto.Title,
                Description = dto.Description,
                RequestedBy = username
            };
            _db.MaintenanceRequests.Add(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPost("{id}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int id, AssignDto dto)
        {
            var req = await _db.MaintenanceRequests.FindAsync(id);
            if (req == null) return NotFound();
            req.AssignedTo = dto.AssignedTo;
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPost("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> Complete(int id)
        {
            var req = await _db.MaintenanceRequests.FindAsync(id);
            if (req == null) return NotFound();
            req.Completed = true;
            req.CompletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(req);
        }
    }

    public record CreateMaintenanceDto(string Title, string? Description);
    public record AssignDto(string AssignedTo);
}