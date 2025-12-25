using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackItAPI.Data;
using TrackItAPI.DTOs.Issue;
using TrackItAPI.Models;

namespace TrackItAPI.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public IssuesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var issues = await _context.Issues.ToListAsync();
            var result = _mapper.Map<List<IssueReadDto>>(issues);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IssueCreateDto dto)
        {
            var issue = _mapper.Map<Issue>(dto);
            issue.CreatedAt = DateTime.Now;
            issue.Status = "Open";

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<IssueReadDto>(issue);
            return Ok(readDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IssueUpdateDto dto)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return NotFound();

            _mapper.Map(dto, issue);
            issue.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<IssueReadDto>(issue));
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
