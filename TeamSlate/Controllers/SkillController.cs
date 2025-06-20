using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamSlate.Data;
using TeamSlate.Models;

namespace TeamSlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Skill
        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            /*
             This endpoint is used by the frontend initially to fetch all skills from 
             SkillMasters Model so the user can see available skills and update them for 
             the resources from the UI if needed.

             The values are retrieved from the Database and not hardcoded on the frontend in
             order to make the code maintainable.
            */
            var skills = await _context.SkillMasters
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            return Ok(skills);
        }

        // GET: api/Skill/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkill(int id)
        {
            var skill = await _context.SkillMasters.FindAsync(id);

            if (skill == null)
                return NotFound();

            return Ok(skill);
        }

        // POST: api/Skill
        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] SkillMaster skill)
        {
            if (skill == null || string.IsNullOrWhiteSpace(skill.Name))
                return BadRequest("Invalid skill input");

            _context.SkillMasters.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, skill);
        }

        // PUT: api/Skill/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] SkillMaster updatedSkill)
        {
            if (id != updatedSkill.Id)
                return BadRequest("ID mismatch");

            var existing = await _context.SkillMasters.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = updatedSkill.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Skill/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.SkillMasters.FindAsync(id);
            if (skill == null)
                return NotFound();

            _context.SkillMasters.Remove(skill);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
