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
    public class ZooAnimalsController : ControllerBase
    {
        private readonly ZooDbContext _context;

        public ZooAnimalsController(ZooDbContext context)
        {
            _context = context;
        }

        // GET: api/ZooAnimals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZooAnimal>>> GetZooAnimals()
        {
            return await _context.ZooAnimals.ToListAsync();
        }

        // GET: api/ZooAnimals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZooAnimal>> GetZooAnimal(int id)
        {
            var zooAnimal = await _context.ZooAnimals.FindAsync(id);

            if (zooAnimal == null)
            {
                return NotFound();
            }

            return zooAnimal;
        }

        // PUT: api/ZooAnimals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZooAnimal(int id, ZooAnimal zooAnimal)
        {
            if (id != zooAnimal.Id)
            {
                return BadRequest();
            }

            _context.Entry(zooAnimal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZooAnimalExists(id))
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

        // POST: api/ZooAnimals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ZooAnimal>> PostZooAnimal(ZooAnimal zooAnimal)
        {
            _context.ZooAnimals.Add(zooAnimal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZooAnimal", new { id = zooAnimal.Id }, zooAnimal);
        }

        // DELETE: api/ZooAnimals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZooAnimal(int id)
        {
            var zooAnimal = await _context.ZooAnimals.FindAsync(id);
            if (zooAnimal == null)
            {
                return NotFound();
            }

            _context.ZooAnimals.Remove(zooAnimal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZooAnimalExists(int id)
        {
            return _context.ZooAnimals.Any(e => e.Id == id);
        }
    }
}
