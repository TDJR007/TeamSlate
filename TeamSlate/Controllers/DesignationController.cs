using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamSlate.Data;
using TeamSlate.Models;

namespace TeamSlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignationController : ControllerBase
    {

        // This is a utlility API to the DesignationMaster table
        private readonly ApplicationDbContext _context;

        public DesignationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Designation
        [HttpGet]
        public async Task<IActionResult> GetDesignations()
        {
            var designations = await _context.DesignationMasters
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return Ok(designations);
        }

        // GET: api/Designation/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignation(int id)
        {
            /* Frontend uses this request to fetch all designations initially so the 
             user can view and update designation for the resource if they want.
            */
            var designation = await _context.DesignationMasters.FindAsync(id);

            if (designation == null)
                return NotFound();

            return Ok(designation);
        }

        // POST: api/Designation
        [HttpPost]
        public async Task<IActionResult> CreateDesignation([FromBody] DesignationMaster designation)
        {
            if (designation == null || string.IsNullOrWhiteSpace(designation.Name))
                return BadRequest("Invalid input");

            _context.DesignationMasters.Add(designation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDesignation), new { id = designation.Id }, designation);
        }

        // PUT: api/Designation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDesignation(int id, [FromBody] DesignationMaster updatedDesignation)
        {
            if (id != updatedDesignation.Id)
                return BadRequest("ID mismatch");

            var existing = await _context.DesignationMasters.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = updatedDesignation.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Designation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var designation = await _context.DesignationMasters.FindAsync(id);
            if (designation == null)
                return NotFound();

            _context.DesignationMasters.Remove(designation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
