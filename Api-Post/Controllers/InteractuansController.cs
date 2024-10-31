using Api_Usuarios.Data;
using Api_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Usuarios.Controllers
{
    [ApiController]
    [Route("api/[Interactuan]")]
    public class InteractuanController : ControllerBase
    {
        private readonly MyDbContext _context;

        public InteractuanController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Interactuan
        [HttpGet]
        public async Task<ActionResult<List<Interactuan>>> GetAll()
        {
            return await _context.Interactuan.ToListAsync();
        }

        // GET: api/Interactuan/Details
        [HttpGet("Details")]
        public async Task<ActionResult<Interactuan>> GetById(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(m => m.IDdeEmisor == IDdeEmisor && m.IDdeReceptor == IDdeReceptor && m.Tipo == Tipo);

            if (interactuan == null)
            {
                return NotFound();
            }

            return Ok(interactuan);
        }

        // POST: api/Interactuan
        [HttpPost]
        public async Task<ActionResult<Interactuan>> Create([FromBody] Interactuan interactuan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interactuan);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { IDdeEmisor = interactuan.IDdeEmisor, IDdeReceptor = interactuan.IDdeReceptor, Tipo = interactuan.Tipo }, interactuan);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Interactuan/Details
        [HttpDelete("Details")]
        public async Task<IActionResult> Delete(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(m => m.IDdeEmisor == IDdeEmisor && m.IDdeReceptor == IDdeReceptor && m.Tipo == Tipo);

            if (interactuan == null)
            {
                return NotFound();
            }

            _context.Interactuan.Remove(interactuan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InteractuanExists(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            return _context.Interactuan.Any(e => e.IDdeEmisor == IDdeEmisor && e.IDdeReceptor == IDdeReceptor && e.Tipo == Tipo);
        }
    }
}
