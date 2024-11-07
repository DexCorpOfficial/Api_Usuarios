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
        [HttpGet("{id}")]
        public async Task<ActionResult<Interactuan>> GetById(int id)
        {
            var interactuan = await _context.Interactuan.FindAsync(id);

            if (interactuan == null)
            {
                return NotFound();
            }

            return Ok(interactuan);
        }

        // POST: Interactuan/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Interactuan>> CreateInteraccion([FromBody] Interactuan interactuan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Depuración: Imprimir los IDs de los emisores y receptores recibidos en la solicitud
                    Console.WriteLine($"ID Emisor recibido: {interactuan.IDdeEmisor}");
                    Console.WriteLine($"ID Receptor recibido: {interactuan.IDdeReceptor}");

                    // Buscar las cuentas emisor y receptor en la base de datos
                    var emisor = await _context.Cuenta.FindAsync(interactuan.IDdeEmisor);
                    var receptor = await _context.Cuenta.FindAsync(interactuan.IDdeReceptor);

                    // Depuración: Verificar si los emisores y receptores se encontraron
                    Console.WriteLine("Emisor encontrado: " + (emisor != null ? $"ID: {emisor.ID}, Nombre: {emisor.Nombre}" : "No encontrado"));
                    Console.WriteLine("Receptor encontrado: " + (receptor != null ? $"ID: {receptor.ID}, Nombre: {receptor.Nombre}" : "No encontrado"));

                    // Verifica si ambos usuarios existen
                    if (emisor == null || receptor == null)
                    {
                        return NotFound("Emisor o receptor no encontrados");
                    }

                    // Crear la interacción de tipo "SeguirCuenta"
                    interactuan.Fecha = DateTime.Now;
                    interactuan.Notificacion = $"{interactuan.IDdeEmisor} ha comenzado a seguir tu cuenta.";
                    interactuan.Seguido = true;
                    interactuan.Emisor = emisor;
                    interactuan.Receptor = receptor;

                    // Agregar la nueva interacción a la base de datos
                    _context.Interactuan.Add(interactuan);
                    await _context.SaveChangesAsync();

                    // Retornar la respuesta con la interacción creada
                    return CreatedAtAction(nameof(CreateInteraccion), new { id = interactuan.ID }, interactuan);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al crear la interacción: {ex.Message}");
                }
            }

            return BadRequest(ModelState);
        }





        // DELETE: Interactuan/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var interactuan = await _context.Interactuan.FindAsync(id);

            if (interactuan == null)
            {
                return NotFound();
            }

            _context.Interactuan.Remove(interactuan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: Interactuan/Seguidores/{idUsuario}
        [HttpGet("Seguidores/{idUsuario}")]
        public async Task<ActionResult<int>> GetSeguidores(int idUsuario)
        {
            var seguidores = await _context.Interactuan
                .Where(i => i.IDdeReceptor == idUsuario && i.Seguido == true)
                .CountAsync();

            return Ok(seguidores);
        }

        // GET: Interactuan/Seguidos/{idUsuario}
        [HttpGet("Seguidos/{idUsuario}")]
        public async Task<ActionResult<int>> GetSeguidos(int idUsuario)
        {
            var seguidos = await _context.Interactuan
                .Where(i => i.IDdeEmisor == idUsuario && i.Seguido == true)
                .CountAsync();

            return Ok(seguidos);
        }
    }
}
