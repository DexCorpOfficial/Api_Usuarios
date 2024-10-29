using Api_Usuarios.Data;
using Api_Usuarios.Models;
using Api_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Usuarios.Controllers
{
    public class InteractuanController : Controller
    {
        private readonly MyDbContext _context;

        public InteractuanController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Interactuan
        public Task<List<Interactuan>> Index()
        {
            return _context.Interactuan.ToListAsync();
        }

        // GET: Interactuan/Details
        public async Task<Interactuan> Details(int? IDdeEmisor, int? IDdeReceptor, string? Tipo)
        {
            if (IDdeEmisor == null || IDdeReceptor == null || Tipo == null)
            {
                return new Interactuan();
            }

            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(m => m.IDdeEmisor == IDdeEmisor && m.IDdeReceptor == IDdeReceptor && m.Tipo == Tipo);

            if (interactuan == null)
            {
                return new Interactuan();
            }

            return interactuan;
        }

        // GET: Interactuan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Interactuan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Interactuan> Create([Bind("IDdeEmisor,IDdeReceptor,Tipo,Fecha,Notificacion,Estado,Contenido,Seguido")] Interactuan interactuan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interactuan);
                await _context.SaveChangesAsync();
                return interactuan;
            }
            return interactuan;
        }

        // GET: Interactuan/Delete
        public async Task<IActionResult> Delete(int? IDdeEmisor, int? IDdeReceptor, string? Tipo)
        {
            if (IDdeEmisor == null || IDdeReceptor == null || Tipo == null)
            {
                return NotFound();
            }

            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(m => m.IDdeEmisor == IDdeEmisor && m.IDdeReceptor == IDdeReceptor && m.Tipo == Tipo);

            if (interactuan == null)
            {
                return NotFound();
            }

            return View(interactuan);
        }

        // POST: Interactuan/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            var interactuan = await _context.Interactuan
                .FirstOrDefaultAsync(m => m.IDdeEmisor == IDdeEmisor && m.IDdeReceptor == IDdeReceptor && m.Tipo == Tipo);

            if (interactuan != null)
            {
                _context.Interactuan.Remove(interactuan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InteractuanExists(int IDdeEmisor, int IDdeReceptor, string Tipo)
        {
            return _context.Interactuan.Any(e => e.IDdeEmisor == IDdeEmisor && e.IDdeReceptor == IDdeReceptor && e.Tipo == Tipo);
        }
    }
}
