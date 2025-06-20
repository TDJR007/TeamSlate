using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamSlate.Models;
using TeamSlate.Data;
using TeamSlate.DTOs;

namespace TeamSlate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyHoursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WeeklyHoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/weeklyhours
        // Optional query: ?week=2025-06-17
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? week)
        {
            var query = _context.WeeklyHours
                .Include(w => w.Resource)
                .AsQueryable();

            if (week.HasValue)
            {
                var monday = week.Value.Date.AddDays(-(int)week.Value.DayOfWeek + (int)DayOfWeek.Monday);
                query = query.Where(w => w.WeekStartDate == monday);
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }

        // GET: api/weeklyhours/by-resource/3
        [HttpGet("by-resource/{resourceId}")]
        public async Task<IActionResult> GetByResource(int resourceId)
        {
            var hours = await _context.WeeklyHours
                .Where(w => w.ResourceId == resourceId)
                .OrderBy(w => w.WeekStartDate)
                .ToListAsync();

            return Ok(hours);
        }

        // POST: api/weeklyhours
        [HttpPost]
        public async Task<IActionResult> Create(WeeklyHour weeklyHour)
        {
            _context.WeeklyHours.Add(weeklyHour);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByResource), new { resourceId = weeklyHour.ResourceId }, weeklyHour);
        }

        // PUT: api/weeklyhours/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WeeklyHour updatedHour)
        {
            if (id != updatedHour.Id)
                return BadRequest();

            var existing = await _context.WeeklyHours.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.WeekStartDate = updatedHour.WeekStartDate;
            existing.Hours = updatedHour.Hours;
            existing.ResourceId = updatedHour.ResourceId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/weeklyhours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.WeeklyHours.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.WeeklyHours.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        



    }

}
