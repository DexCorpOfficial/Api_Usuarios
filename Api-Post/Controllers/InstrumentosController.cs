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

        public class InstrumentoDto
        {
            public int IDdeCuenta { get; set; }
            public string Instrumento { get; set; }
        }

        // GET: Instrumentos
        [HttpGet]
        public async Task<ActionResult<List<InstrumentoDto>>> GetAll()
        {
            var instrumentos = await _context.Instrumentos.ToListAsync();

            var instrumentosDto = instrumentos.Select(i => new InstrumentoDto
            {
                IDdeCuenta = i.IDdeCuenta,
                Instrumento = i.Instrumento
            }).ToList();

            return Ok(instrumentosDto);
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

        // PUT: Instrumentos/Edit/{id}/{instrumento}
        [HttpPut("edit/{id}/{instrumento}")]
        public async Task<IActionResult> Edit(int id, string instrumento, [FromBody] Instrumentos updatedInstrumento)
        {
            // Busca el instrumento que corresponde al IDdeCuenta
            var instrumentoExistente = await _context.Instrumentos
                .FirstOrDefaultAsync(i => i.IDdeCuenta == id);

            if (instrumentoExistente == null)
            {
                return NotFound();
            }

            // Actualiza el instrumento directamente
            instrumentoExistente.Instrumento = updatedInstrumento.Instrumento;

            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content si la actualización fue exitosa
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
