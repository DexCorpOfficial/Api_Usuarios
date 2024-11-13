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

        // GET: Interactuan/BuscarInteraccion/{idEmisor}/{idReceptor}
        [HttpGet("BuscarInteraccion/{idEmisor}/{idReceptor}")]
        public async Task<ActionResult<Interactuan>> BuscarInteraccion(int idEmisor, int idReceptor)
        {
            var interaccion = await _context.Interactuan
                .Where(i => i.IDdeEmisor == idEmisor && i.IDdeReceptor == idReceptor && i.Seguido == true)
                .FirstOrDefaultAsync();

            if (interaccion == null)
            {
                return NotFound("Interacción no encontrada.");
            }

            return Ok(interaccion);
        }


        // GET: Interactuan/Seguidores/{idUsuario}
        [HttpGet("Seguidores/{idUsuario}")]
        public async Task<ActionResult<int>> GetSeguidores(int idUsuario)
        {
            var seguidoresCount = await _context.Interactuan
                .Where(i => i.IDdeReceptor == idUsuario && i.Seguido == true)
                .CountAsync();

            return Ok(seguidoresCount);
        }

        // GET: Interactuan/Seguidos/{idUsuario}
        [HttpGet("Seguidos/{idUsuario}")]
        public async Task<ActionResult<int>> GetSeguidos(int idUsuario)
        {
            var seguidosCount = await _context.Interactuan
                .Where(i => i.IDdeEmisor == idUsuario && i.Seguido == true)
                .CountAsync();

            return Ok(seguidosCount);
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


        [HttpGet("VerificarInteraccion/{idEmisor}/{idReceptor}")]
        public async Task<ActionResult<bool>> VerificarInteraccion(int idEmisor, int idReceptor)
        {
            var existe = await _context.Interactuan
                .AnyAsync(i => i.IDdeEmisor == idEmisor && i.IDdeReceptor == idReceptor && i.Seguido == true);

            return Ok(existe);
        }

        [HttpPost("EnviarMensaje")]
        public async Task<IActionResult> EnviarMensaje([FromBody] Interactuan mensaje)
        {   
            _context.Interactuan.Add(mensaje);
            await _context.SaveChangesAsync();

            return Ok(mensaje);
        }

        [HttpGet("ObtenerMensajes/{idUsuario1}/{idUsuario2}")]
        public async Task<IActionResult> ObtenerMensajes(int idUsuario1, int idUsuario2)
        {
            // Si idUsuario2 es 0, devolver todos los mensajes relacionados con idUsuario1
            IQueryable<Interactuan> mensajesQuery;

            if (idUsuario2 == 0)
            {
                // Obtener todos los mensajes enviados o recibidos por idUsuario1
                mensajesQuery = _context.Interactuan
                    .Where(m => (m.IDdeEmisor == idUsuario1 || m.IDdeReceptor == idUsuario1) && m.Seguido == false)
                    .OrderBy(m => m.Fecha);
            }
            else
            {
                // Obtener mensajes entre idUsuario1 e idUsuario2
                mensajesQuery = _context.Interactuan
                    .Where(m => (m.IDdeEmisor == idUsuario1 && m.IDdeReceptor == idUsuario2 && m.Seguido == false) ||
                                (m.IDdeEmisor == idUsuario2 && m.IDdeReceptor == idUsuario1 && m.Seguido == false))
                    .OrderBy(m => m.Fecha);
            }

            var mensajes = await mensajesQuery.ToListAsync();

            return Ok(mensajes);
        }


        [HttpPost("LeerMensaje/{mensajeId}")]
        public async Task<IActionResult> LeerMensaje(int mensajeId)
        {
            // Buscar el mensaje por su ID
            var mensaje = await _context.Interactuan
                .FirstOrDefaultAsync(m => m.ID == mensajeId); // Ajusta 'ID' si tu campo tiene otro nombre

            if (mensaje == null)
            {
                // Si el mensaje no se encuentra, simplemente se devuelve un 404 sin detalles adicionales
                return NotFound();
            }

            // Cambiar el estado a "Leído"
            if (mensaje.Estado == "Enviado")
            {
                mensaje.Estado = "Leído";
                _context.Update(mensaje);
                await _context.SaveChangesAsync();

                // Respuesta exitosa sin detalles adicionales
                return Ok();
            }

            // Si el mensaje ya está marcado como "Leído", se devuelve un 400 sin mensaje adicional
            return BadRequest();
        }




        // DELETE: Interactuan/Delete/{idInteraccion}
        // DELETE: Interactuan/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Buscar la interacción usando la ID
                var interaccion = await _context.Interactuan
                    .Where(i => i.ID == id)
                    .FirstOrDefaultAsync();

                if (interaccion == null)
                {
                    return NotFound("Interacción no encontrada.");
                }

                // Eliminar la interacción
                _context.Interactuan.Remove(interaccion);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la interacción: {ex.Message}");
            }
        }



    }
}
