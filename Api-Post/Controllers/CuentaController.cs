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
    [Route("api/[Cuenta]")]
    public class CuentaController : ControllerBase // Cambia a ControllerBase
    {
        private readonly MyDbContext _context;

        public CuentaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Cuenta
        [HttpGet]
        public async Task<ActionResult<List<Cuenta>>> GetAll()
        {
            return await _context.Cuenta.ToListAsync();
        }

        // GET: api/Cuenta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetById(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra la cuenta
            }

            return Ok(cuenta); // Devuelve la cuenta encontrada
        }

        // POST: api/Cuenta
        [HttpPost]
        public async Task<ActionResult<Cuenta>> Create([FromBody] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                // Asigna la fecha de creación y el estado activo
                cuenta.Fecha_Creacion = DateTime.Now;
                cuenta.Activo = true;

                _context.Cuenta.Add(cuenta);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = cuenta.ID }, cuenta); // Devuelve 201 con la nueva cuenta creada
            }

            return BadRequest(ModelState); // Devuelve 400 si el modelo es inválido
        }

        // PUT: api/Cuenta/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Cuenta cuentaActualizada)
        {
            var cuentaExistente = await _context.Cuenta.FindAsync(id);
            if (cuentaExistente == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra la cuenta
            }

            if (ModelState.IsValid)
            {
                // Actualiza solo los campos permitidos
                cuentaExistente.genero = cuentaActualizada.genero;
                cuentaExistente.Nombre = cuentaActualizada.Nombre;
                cuentaExistente.foto_perfil = cuentaActualizada.foto_perfil;
                cuentaExistente.Biografia = cuentaActualizada.Biografia;
                cuentaExistente.fecha_nac = cuentaActualizada.fecha_nac;
                cuentaExistente.Musico = cuentaActualizada.Musico;
                cuentaExistente.Contrasenia = cuentaActualizada.Contrasenia;
                cuentaExistente.Privado = cuentaActualizada.Privado;

                await _context.SaveChangesAsync();
                return NoContent(); // Devuelve 204 si la actualización fue exitosa
            }

            return BadRequest(ModelState); // Devuelve 400 si el modelo es inválido
        }

        // DELETE: api/Cuenta/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra la cuenta
            }

            cuenta.Activo = false; // Desactiva la cuenta sin eliminarla
            _context.Cuenta.Update(cuenta);
            await _context.SaveChangesAsync();

            return NoContent(); // Devuelve 204 para indicar que la operación fue exitosa
        }

        private bool CuentaExists(int id)
        {
            return _context.Cuenta.Any(e => e.ID == id);
        }
    }
}
