using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamSlate.Data;
using TeamSlate.DTOs;
using TeamSlate.Models;

namespace TeamSlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResourceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // === POST: Add Resource ===
        [HttpPost("add-resource")]
        public async Task<IActionResult> AddResource([FromBody] AddResourceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = new Resource
            {
                Name = dto.Name,
                DesignationId = dto.DesignationId,
                BillableId = dto.BillableId,
                Availability = dto.Availability
            };

            // Add Skills
            foreach (var skill in dto.ResourceSkills)
            {
                resource.ResourceSkills.Add(new ResourceSkill
                {
                    SkillId = skill.SkillId,
                    Resource = resource
                });
            }

            // Add Weekly Hours
            foreach (var week in dto.WeeklyHours)
            {
                resource.WeeklyHours.Add(new WeeklyHour
                {
                    WeekStartDate = week.WeekStartDate,
                    Hours = week.Hours,
                    Resource = resource
                });
            }

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResourceById), new { id = resource.Id }, resource);
        }

        // === GET: Resource by ID ===
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResourceById(int id)
        {
            var resource = await _context.Resources
                .Include(r => r.Designation)
                .Include(r => r.Billable)
                .Include(r => r.WeeklyHours)
                .Include(r => r.ResourceSkills)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        // === DELETE: Resource ===
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
                return NotFound();

            // Remove related skills and hours
            _context.ResourceSkills.RemoveRange(_context.ResourceSkills.Where(rs => rs.ResourceId == id));
            _context.WeeklyHours.RemoveRange(_context.WeeklyHours.Where(w => w.ResourceId == id));

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // === GET: All Resources (for grid) ===
        [HttpGet("with-hours")]
        public async Task<IActionResult> GetResourcesWithHours([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var resources = await _context.Resources
                .Include(r => r.ResourceSkills).ThenInclude(rs => rs.Skill)
                .Include(r => r.WeeklyHours)
                .Include(r => r.Designation)
                .Include(r => r.Billable)
                .ToListAsync();

            // Get Mondays in date range
            var weeks = Enumerable.Range(0, (end - start).Days + 1)
                .Select(i => start.AddDays(i))
                .Where(d => d.DayOfWeek == DayOfWeek.Monday)
                .Select(d => d.Date)
                .ToList();

            var response = resources.Select(r => new
            {
                r.Id,
                r.Name,
                r.BillableId,
                Billable = r.Billable?.Label,
                r.DesignationId,
                Designation = r.Designation?.Name,
                r.Availability,
                Skills = r.ResourceSkills.Select(rs => new { rs.SkillId, rs.Skill.Name }),
                WeeklyHours = weeks.Select(w => new
                {
                    WeekStartDate = w,
                    Hours = r.WeeklyHours.FirstOrDefault(h => h.WeekStartDate == w)?.Hours ?? 0
                })
            });

            return Ok(response);
        }

        // === PUT: Update Resource With Hours (Grid Save) ===
        [HttpPut("update-resource-with-hours")]
        public async Task<IActionResult> UpdateResourceWithHours([FromBody] UpdateResourceWithHoursDto dto)
        {
            var resource = await _context.Resources
                .Include(r => r.ResourceSkills)
                .Include(r => r.WeeklyHours)
                .FirstOrDefaultAsync(r => r.Id == dto.ResourceId);

            if (resource == null)
                return NotFound("Resource not found.");

            // Update base info
            resource.Name = dto.Name;
            resource.DesignationId = dto.DesignationId;
            resource.BillableId = dto.BillableId;
            resource.Availability = dto.Availability;

            // Replace skills
            _context.ResourceSkills.RemoveRange(resource.ResourceSkills);
            foreach (var skillId in dto.SkillIds)
            {
                resource.ResourceSkills.Add(new ResourceSkill
                {
                    ResourceId = resource.Id,
                    SkillId = skillId
                });
            }

            // Upsert Weekly Hours
            foreach (var wh in dto.WeeklyHours)
            {
                var existing = resource.WeeklyHours.FirstOrDefault(h => h.WeekStartDate == wh.WeekStartDate);
                if (existing != null)
                    existing.Hours = wh.Hours;
                else
                    resource.WeeklyHours.Add(new WeeklyHour
                    {
                        ResourceId = resource.Id,
                        WeekStartDate = wh.WeekStartDate,
                        Hours = wh.Hours
                    });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // === GET: All Resources (for testing) ===
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources
                .Include(r => r.Designation)
                .Include(r => r.Billable)
                .Include(r => r.ResourceSkills).ThenInclude(rs => rs.Skill)
                .ToListAsync();
        }

        // === GET: Master Data (testing) ===
        [HttpGet("masters")]
        public async Task<IActionResult> GetMasterData()
        {
            var designations = await _context.DesignationMasters.ToListAsync();
            var billables = await _context.BillableMasters.ToListAsync();
            var skills = await _context.SkillMasters.ToListAsync();

            return Ok(new
            {
                designations,
                billables,
                skills
            });
        }

        // === PUT: Update Resource (testing only) ===
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource(int id, Resource resource)
        {
            if (id != resource.Id)
                return BadRequest();

            var oldSkills = _context.ResourceSkills.Where(rs => rs.ResourceId == id);
            _context.ResourceSkills.RemoveRange(oldSkills);
            await _context.SaveChangesAsync();

            _context.Entry(resource).State = EntityState.Modified;

            if (resource.ResourceSkills != null)
            {
                foreach (var rs in resource.ResourceSkills)
                {
                    rs.ResourceId = id;
                    _context.ResourceSkills.Add(rs);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Resources.Any(r => r.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }
    }
}
