using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreeApi.Data;
using TreeApi.Models;

namespace TreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        private readonly TreeDbContext _context;

        public TreeController(TreeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreeNode>>> GetTree()
        {
            var nodes = await _context.TreeNodes
                .Include(t => t.Children)
                .ToListAsync();

            return Ok(nodes.Where(t => t.ParentId == null));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TreeNode>> GetNode(int id)
        {
            var node = await _context.TreeNodes
                .Include(t => t.Children)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (node == null)
                return NotFound();

            return Ok(node);
        }

        [HttpPost]
        public async Task<ActionResult<TreeNode>> CreateNode(TreeNode node)
        {
            _context.TreeNodes.Add(node);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNode), new { id = node.Id }, node);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNode(int id, TreeNode node)
        {
            if (id != node.Id)
                return BadRequest();

            _context.Entry(node).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TreeNodes.Any(t => t.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNode(int id)
        {
            var node = await _context.TreeNodes.FindAsync(id);

            if (node == null)
                return NotFound();

            _context.TreeNodes.Remove(node);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
