using Api_Post.Data;
using Api_Post.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Post.Controllers
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
        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Cuenta.ToListAsync());
        }*/

        // GET: Cuenta/Details/5
        public async Task<Cuenta> Details(int? id)
        {
            if (id == null)
            {
                return new Cuenta();
            }

            var Cuenta = await _context.Cuenta
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<Cuenta> Create([Bind("Id, Nombre, FotoPerfil, Biografia, Seguidores, Genero, Seguidos, FechaCreacion, FechaNacimiento, Musico, Activo, Contrasena, Privado")] Cuenta Cuenta)
        {
            //https://sentry.io/answers/how-to-send-a-post-request-in-net-using-c-sharp/
            //curl -X POST https://localhost:5001/Cuenta/create -H "Content-Type: application/json" -d '{"titulo":"Alice","descripcion":"descripciondeprueba","idDeUsuario":5}'
            if (ModelState.IsValid)
            {



                _context.Add(Cuenta);
                await _context.SaveChangesAsync();
                return Cuenta;
            }
            return Cuenta;
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
        public async Task<IActionResult> Editar(int id, [Bind("Id, Nombre, FotoPerfil, Biografia, Seguidores, Genero, Seguidos, FechaCreacion, FechaNacimiento, Musico, Activo, Contrasena, Privado")] Cuenta cuenta)
        {
            if (id != cuenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Cuenta.Update(cuenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuenta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirigir a la acción Details después de guardar los cambios
                return RedirectToAction(nameof(Index));
            }
            return View(cuenta);
        }


        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Cuenta = await _context.Cuenta
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return _context.Cuenta.Any(e => e.Id == id);
        }
    }
}
