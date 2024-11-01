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
    [Route("Interactuan")]
    public class InteractuanController : ControllerBase
    {
        private readonly MyDbContext _context;

        public InteractuanController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Interactuan
        [HttpGet]
        public async Task<ActionResult<List<Interactuan>>> GetAll()
        {
            return await _context.Interactuan.ToListAsync();
        }

        // GET: Interactuan/Details
        [HttpGet("{IDdeEmisor}/{IDdeReceptor}/{Tipo}")]
        public async Task<ActionResult<Interactuan>> GetById(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(i => i.IDdeEmisor == IDdeEmisor && i.IDdeReceptor == IDdeReceptor && i.Tipo == Tipo);

            if (interactuan == null)
            {
                return NotFound();
            }

            return Ok(interactuan);
        }

        // POST: Interactuan/Create
        [HttpPost("create")]
        public async Task<ActionResult<Interactuan>> Create([FromBody] Interactuan interactuan)
        {
            if (ModelState.IsValid)
            {
                _context.Interactuan.Add(interactuan);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { IDdeEmisor = interactuan.IDdeEmisor, IDdeReceptor = interactuan.IDdeReceptor, Tipo = interactuan.Tipo }, interactuan);
            }

            return BadRequest(ModelState);
        }

        // DELETE: Interactuan/Details
        [HttpDelete("{IDdeEmisor}/{IDdeReceptor}/{Tipo}")]
        public async Task<IActionResult> Delete(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(i => i.IDdeEmisor == IDdeEmisor && i.IDdeReceptor == IDdeReceptor && i.Tipo == Tipo);

            if (interactuan == null)
            {
                return NotFound();
            }

            _context.Interactuan.Remove(interactuan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
