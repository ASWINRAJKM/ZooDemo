using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooDemoAPI.Data;
using ZooDemoAPI.Model;

namespace ZooDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoosController : ControllerBase
    {
        private readonly ZooDbContext _context;

        public ZoosController(ZooDbContext context)
        {
            _context = context;
        }

        // GET: api/Zoos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zoo>>> GetZoos()
        {
            return await _context.Zoos.ToListAsync();
        }

        // GET: api/Zoos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Zoo>> GetZoo(int id)
        {
            var zoo = await _context.Zoos.FindAsync(id);

            if (zoo == null)
            {
                return NotFound();
            }

            return zoo;
        }

        // PUT: api/Zoos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZoo(int id, Zoo zoo)
        {
            if (id != zoo.Id)
            {
                return BadRequest();
            }

            _context.Entry(zoo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZooExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Zoos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Zoo>> PostZoo(Zoo zoo)
        {
            _context.Zoos.Add(zoo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZoo", new { id = zoo.Id }, zoo);
        }

        // DELETE: api/Zoos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZoo(int id)
        {
            var zoo = await _context.Zoos.FindAsync(id);
            if (zoo == null)
            {
                return NotFound();
            }

            _context.Zoos.Remove(zoo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZooExists(int id)
        {
            return _context.Zoos.Any(e => e.Id == id);
        }
    }
}
