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
    [Route("Cuenta")]
    public class CuentaController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CuentaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Cuenta
        [HttpGet]
        public async Task<ActionResult<List<Cuenta>>> GetAll()
        {
            return await _context.Cuenta.ToListAsync();
        }

        // GET: Cuenta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetById(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return Ok(cuenta);
        }

        // POST: Cuenta/Create
        [HttpPost("create")]
        public async Task<ActionResult<Cuenta>> Create([Bind("ID, Nombre, genero, foto_perfil, Biografia, fecha_nac, Musico, Contrasenia, Privado")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                // Verifica si 'foto_perfil' está vacío y asigna la imagen predeterminada si es necesario
                if (cuenta.foto_perfil == null || cuenta.foto_perfil.Length == 0)
                {
                    cuenta.foto_perfil = Properties.Resources.Foto_de_Perfil_Por_Defecto; // Asigna la imagen por defecto
                }

                cuenta.Fecha_Creacion = DateTime.Now;
                cuenta.Activo = true;
                _context.Add(cuenta);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = cuenta.ID }, cuenta);
            }

            return BadRequest(ModelState);
        }

        // PUT: Cuenta/Edit/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Nombre, genero, foto_perfil, Biografia, fecha_nac, Musico, Contrasenia, Privado")] Cuenta cuentaActualizada)
        {
            if (id != cuentaActualizada.ID)
            {
                return BadRequest();
            }

            var cuentaExistente = await _context.Cuenta.FindAsync(id);
            if (cuentaExistente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualiza solo los campos permitidos
                    cuentaExistente.genero = cuentaActualizada.genero;
                    cuentaExistente.Nombre = cuentaActualizada.Nombre;

                    // Solo actualiza la foto si se proporciona una nueva
                    if (cuentaActualizada.foto_perfil != null && cuentaActualizada.foto_perfil.Length > 0)
                    {
                        cuentaExistente.foto_perfil = cuentaActualizada.foto_perfil;
                    }

                    cuentaExistente.Biografia = cuentaActualizada.Biografia;
                    cuentaExistente.fecha_nac = cuentaActualizada.fecha_nac;
                    cuentaExistente.Musico = cuentaActualizada.Musico;
                    cuentaExistente.Contrasenia = cuentaActualizada.Contrasenia;
                    cuentaExistente.Privado = cuentaActualizada.Privado;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuentaExistente.ID))
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

            return BadRequest(ModelState);
        }

        // DELETE: Cuenta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            // Cambia el estado de la cuenta a inactiva
            cuenta.Activo = false;
            _context.Cuenta.Update(cuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: Cuenta/5/actualizar_privacidad
        [HttpPut("actualizar_privacidad/{id}")]
        public async Task<IActionResult> ActualizarPrivacidad(int id, [FromBody] bool privacidad)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            cuenta.Privado = privacidad;  // Actualiza el campo 'Privado'

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Privacidad actualizada correctamente", cuenta });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la privacidad", error = ex.Message });
            }
        }

        // PUT: Cuenta/CambiarContrasenia/{id}
        [HttpPut("CambiarContrasenia/{id}")]
        public async Task<IActionResult> CambiarContrasenia(int id, [FromBody] CambiarContraseniaDto cambiarContraseniaDto)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Validar la contraseña anterior
            if (cuenta.Contrasenia != cambiarContraseniaDto.ContraseniaAnterior)
            {
                return BadRequest("La contraseña anterior es incorrecta.");
            }

            // Cambiar la contraseña
            cuenta.Contrasenia = cambiarContraseniaDto.ContraseniaNueva;
            await _context.SaveChangesAsync();

            return NoContent(); // Cambio exitoso
        }

        // DTO para recibir los datos de cambio de contraseña
        public class CambiarContraseniaDto
        {
            public string ContraseniaAnterior { get; set; }
            public string ContraseniaNueva { get; set; }
        }


        private bool CuentaExists(int id)
        {
            return _context.Cuenta.Any(e => e.ID == id);
        }
    }
}
