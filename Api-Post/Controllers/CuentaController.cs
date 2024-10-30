using Api_Usuarios.Data;
using Api_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Usuarios.Controllers
{
    public class CuentaController : Controller
    {
        private readonly MyDbContext _context;

        public CuentaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Cuenta

        public Task<List<Cuenta>> Index()
        {
            return _context.Cuenta.ToListAsync();
        }

        // GET: Cuenta/Details/5
        public async Task<Cuenta> Details(int? id)
        {
            if (id == null)
            {
                return new Cuenta();
            }

            var Cuenta = await _context.Cuenta
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Cuenta == null)
            {
                return new Cuenta();
            }

            return Cuenta;
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Cuenta> Create([Bind("ID, Nombre, genero, foto_perfil, Biografia, fecha_nac, fecha_creacion, Musico, activo, Contrasenia, Privado")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                // Verifica si 'foto_perfil' está vacío y asigna la imagen predeterminada si es necesario
                if (cuenta.foto_perfil == null || cuenta.foto_perfil.Length == 0)
                {
                        cuenta.foto_perfil = Properties.Resources.Foto_de_Perfil_Por_Defecto;
                }

                cuenta.Fecha_Creacion = DateTime.Now;
                cuenta.Activo = true;
                _context.Add(cuenta);
                await _context.SaveChangesAsync();

                return cuenta;
            }

            return cuenta;
        }




        // GET: Cuenta/Edit/5
        public IActionResult Edit()
        {
            return View();
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<Cuenta> Edit(int id, [Bind("ID, Nombre, genero, foto_perfil, Biografia, fecha_nac, Musico, Contrasenia, Privado")] Cuenta cuentaActualizada)
        {
            // Busca la cuenta existente en la base de datos usando el ID recibido
            var cuentaExistente = await _context.Cuenta.FindAsync(id);

            // Verifica si la cuenta no existe y retorna un error si es necesario
            if (cuentaExistente == null)
            {
                return new Cuenta(); // Maneja el error o redirecciona como prefieras
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualiza solo los campos permitidos sin tocar el ID, fecha de creación o activo
                    cuentaExistente.genero = cuentaActualizada.genero;
                    cuentaExistente.Nombre = cuentaActualizada.Nombre;
                    cuentaExistente.foto_perfil = cuentaActualizada.foto_perfil;
                    cuentaExistente.Biografia = cuentaActualizada.Biografia;
                    cuentaExistente.fecha_nac = cuentaActualizada.fecha_nac;
                    cuentaExistente.Musico = cuentaActualizada.Musico;
                    cuentaExistente.Contrasenia = cuentaActualizada.Contrasenia;
                    cuentaExistente.Privado = cuentaActualizada.Privado;

                    // Guarda los cambios
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuentaExistente.ID))
                    {
                        return new Cuenta(); // Manejo de error si la cuenta no existe
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return cuentaExistente;
        }


        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cuenta = await _context.Cuenta
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Cuenta == null)
            {
                return NotFound();
            }

            return View(Cuenta);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta != null)
            {
                cuenta.Activo = false;
                _context.Cuenta.Update(cuenta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirige al índice después de eliminar
        }

        private bool CuentaExists(int id)
        {
            return _context.Cuenta.Any(e => e.ID == id);
        }
    }
}
