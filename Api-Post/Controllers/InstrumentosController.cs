using Api_Usuarios.Data;
using Api_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Usuarios.Controllers
{
    [ApiController]
    [Route("Instrumentos")]
    public class InstrumentoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public InstrumentoController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Instrumentos
        [HttpGet]
        public async Task<ActionResult<List<Instrumentos>>> GetAll()
        {
            return await _context.Instrumentos.ToListAsync();
        }

        // GET: Instrumentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Instrumentos>>> GetByCuentaId(int id)
        {
            var instrumentos = await _context.Instrumentos
                .Where(i => i.IDdeCuenta == id)
                .ToListAsync();

            if (instrumentos == null || !instrumentos.Any())
            {
                return NotFound();
            }

            return Ok(instrumentos);
        }

        // POST: Instrumentos/Create
        [HttpPost("create")]
        public async Task<ActionResult<Instrumentos>> Create([Bind("IDdeCuenta, Instrumento")] Instrumentos instrumento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrumento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetByCuentaId), new { id = instrumento.IDdeCuenta }, instrumento);
            }

            return BadRequest(ModelState);
        }

        // PUT: Instrumentos/Edit
        [HttpPut("edit/{id}/{instrumento}")]
        public async Task<IActionResult> Edit(int id, string instrumento, [Bind("Instrumento")] Instrumentos updatedInstrumento)
        {
            if (id != updatedInstrumento.IDdeCuenta || instrumento != updatedInstrumento.Instrumento)
            {
                return BadRequest();
            }

            var instrumentoExistente = await _context.Instrumentos
                .FirstOrDefaultAsync(i => i.IDdeCuenta == id && i.Instrumento == instrumento);
            if (instrumentoExistente == null)
            {
                return NotFound();
            }

            // Actualiza el instrumento
            instrumentoExistente.Instrumento = updatedInstrumento.Instrumento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstrumentoExists(id, instrumento))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: Instrumentos/Delete/{id}?instrumento={nombreInstrumento}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string instrumento)
        {
            var instrumentoExistente = await _context.Instrumentos
                .FirstOrDefaultAsync(i => i.IDdeCuenta == id && i.Instrumento == instrumento);
            if (instrumentoExistente == null)
            {
                return NotFound();
            }

            _context.Instrumentos.Remove(instrumentoExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstrumentoExists(int id, string instrumento)
        {
            return _context.Instrumentos.Any(e => e.IDdeCuenta == id && e.Instrumento == instrumento);
        }
    }
}
