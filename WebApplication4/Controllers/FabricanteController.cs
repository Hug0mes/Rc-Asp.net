using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NToastNotify;
using WebApplication4.Models;
using WebApplication4.Data;

namespace ef2.Controllers
{
    public class FabricantesController : Controller
    {
        private readonly LojaContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IToastNotification _toastNotification;
        private readonly string _imageFolder;

        public FabricantesController(LojaContext context, IWebHostEnvironment appEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _toastNotification = toastNotification;
            _imageFolder = Path.Combine(_appEnvironment.WebRootPath, "images\\Fabricantes");
        }

        // GET: Fabricantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fabricantes.ToListAsync());
        }

        // GET: Fabricantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Fabricantes = await _context.Fabricantes
                .FirstOrDefaultAsync(c => c.Id == id);
            if (Fabricantes == null)
            {
                return NotFound();
            }

            return View(Fabricantes);
        }

        // GET: Fabricantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fabricantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FabricanteViewModel FabricanteVM)
        {
            if (ModelState.IsValid)
            {
                await SaveFabricante(FabricanteVM);
                _toastNotification.AddSuccessToastMessage($"Fabricante {FabricanteVM.Fabricante.Nome} inserido com sucesso");
                return RedirectToAction(nameof(Index));
            }
            return View(FabricanteVM);
        }

        // GET: Fabricantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Fabricante = await _context.Fabricantes.FindAsync(id);
            if (Fabricante == null)
            {
                return NotFound();
            }
           FabricanteViewModel FabricanteVM = new FabricanteViewModel
            {
                Fabricante = Fabricante
            };
            return View(FabricanteVM);
        }

        // POST: Fabricantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FabricanteViewModel FabricanteVM)
        {
            if (id != FabricanteVM.Fabricante.Id)
            {
                return NotFound();
            }
            {
                if (ModelState.IsValid)
                {
                    await SaveFabricante(FabricanteVM);
                    _toastNotification.AddSuccessToastMessage($"Fabricante {FabricanteVM.Fabricante.Nome} alterada com sucesso");
                    return RedirectToAction(nameof(Index));
                }
                return View(FabricanteVM);
            }
        }

        // GET: Fabricantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Fabricante = await _context.Fabricantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Fabricante == null)
            {
                return NotFound();
            }

            return View(Fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Fabricante = await _context.Fabricantes.FindAsync(id);
            await DeleteFabricante(Fabricante);
            return RedirectToAction(nameof(Index));
        }

        private bool FabricanteExists(int id)
        {
            return _context.Fabricantes.Any(e => e.Id == id);
        }


        // Refactor to Repository
        private async Task<bool> SaveFabricante(FabricanteViewModel FabricanteVM)
        {
            try
            {
                // File included?
                string uniqueFileName = UploadedFile(FabricanteVM);
                if ((uniqueFileName != null) || (FabricanteVM.RemoverImagem))
                {
                    // Previous image?
                    if (FabricanteVM.Fabricante.Imagem != null)
                    {
                        // remove file
                        string filePath = Path.Combine(_imageFolder, FabricanteVM.Fabricante.Imagem);
                        System.IO.File.Delete(filePath);

                    }
                    // Set filename image in Artigo
                    FabricanteVM.Fabricante.Imagem = uniqueFileName;
                }

                // Save Fabricante
                if (FabricanteVM.Fabricante.Id == 0)
                {
                    //_context.Entry(Fabricante.Fabricante).State = EntityState.Added;
                    _context.Add(FabricanteVM.Fabricante);
                }
                else
                {
                    //_context.Entry(Fabricante.Fabricante).State = EntityState.Modified;
                    _context.Update(FabricanteVM.Fabricante);
                }
                // Store everything in db
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // TODO: Handle failure
                return false;
            }
            //catch (DbUpdateConcurrencyException)
            //{
            //}
        }

        // Refactor to Repository
        private async Task<bool> DeleteFabricante(Fabricante Fabricante)
        {
            try
            {
                // File included?
                if (Fabricante.Imagem != null)
                {
                    // remove file
                    string filePath = Path.Combine(_imageFolder, Fabricante.Imagem);
                    System.IO.File.Delete(filePath);

                }

                //Dependencies...?
                //if (Fabricante.Artigos.Count > 0)
                //{
                //    // Existem Artigos que referem esta Fabricante, não pode apagar
                //    throw new Exception("Existem Artigos que utilizam esta Fabricante... não pode remover!");
                //}

                _context
                    .Fabricantes
                    .Remove(Fabricante);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // TODO: Handle failure
                return false;
            }
        }

        private string UploadedFile(FabricanteViewModel FabricanteVM)
        {
            string uniqueFileName = null;

            if (FabricanteVM.FicheiroImagem != null)
            {
                System.IO.Directory.CreateDirectory(_imageFolder);
                uniqueFileName = Guid.NewGuid().ToString()
                        + "_"
                        + System.IO.Path.GetFileName(FabricanteVM.FicheiroImagem.FileName);
                string filePath = Path.Combine(_imageFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    FabricanteVM.FicheiroImagem.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }

}
