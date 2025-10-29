using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackItAPI.Data;
using TrackItAPI.Models;

namespace TrackItAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IssuesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Issues.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return Ok(issue);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Issue updated)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return NotFound();

            issue.Title = updated.Title;
            issue.Description = updated.Description;
            issue.Status = updated.Status;
            issue.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(issue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return NotFound();

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
