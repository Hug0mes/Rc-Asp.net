using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ef_2.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NToastNotify;


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
            _imageFolder = Path.Combine(_appEnvironment.WebRootPath, "images\\fabricantes");
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

            var fabricantes = await _context.fabricantes
                .FirstOrDefaultAsync(c => c.Id == id);
            if (fabricantes == null)
            {
                return NotFound();
            }

            return View(fabricante);
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
        public async Task<IActionResult> Create(fabricanteViewModel fabricanteVM)
        {
            if (ModelState.IsValid)
            {
                await Savefabricante(fabricanteVM);
                _toastNotification.AddSuccessToastMessage($"fabricante {fabricanteVM.fabricante.Nome} inserido com sucesso");
                return RedirectToAction(nameof(Index));
            }
            return View(fabricanteVM);
        }

        // GET: Fabricantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricante = await _context.fabricantes.FindAsync(id);
            if (fabricante == null)
            {
                return NotFound();
            }
            fabricanteViewModel fabricanteVM = new fabricanteViewModel
            {
                fabricante = fabricante
            };
            return View(fabricanteVM);
        }

        // POST: Fabricantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, fabricanteViewModel fabricanteVM)
        {
            if (id != fabricanteVM.fabricante.Id)
            {
                return NotFound();
            }
            {
                if (ModelState.IsValid)
                {
                    await Savefabricante(fabricanteVM);
                    _toastNotification.AddSuccessToastMessage($"fabricante {fabricanteVM.fabricante.Nome} alterada com sucesso");
                    return RedirectToAction(nameof(Index));
                }
                return View(fabricanteVM);
            }
        }

        // GET: Fabricantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricante = await _context.fabricantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabricante == null)
            {
                return NotFound();
            }

            return View(fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);
            await Deletefabricante(fabricante);
            return RedirectToAction(nameof(Index));
        }

        private bool fabricanteExists(int id)
        {
            return _context.Fabricantes.Any(e => e.Id == id);
        }


        // Refactor to Repository
        private async Task<bool> Savefabricante(fabricanteViewModel fabricanteVM)
        {
            try
            {
                // File included?
                string uniqueFileName = UploadedFile(fabricanteVM);
                if ((uniqueFileName != null) || (fabricanteVM.RemoverImagem))
                {
                    // Previous image?
                    if (fabricanteVM.fabricante.Imagem != null)
                    {
                        // remove file
                        string filePath = Path.Combine(_imageFolder, fabricanteVM.fabricante.Imagem);
                        System.IO.File.Delete(filePath);

                    }
                    // Set filename image in Artigo
                    fabricanteVM.fabricante.Imagem = uniqueFileName;
                }

                // Save fabricante
                if (fabricanteVM.fabricante.Id == 0)
                {
                    //_context.Entry(fabricante.fabricante).State = EntityState.Added;
                    _context.Add(fabricanteVM.fabricante);
                }
                else
                {
                    //_context.Entry(fabricante.fabricante).State = EntityState.Modified;
                    _context.Update(fabricanteVM.fabricante);
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
        private async Task<bool> Deletefabricante(fabricante fabricante)
        {
            try
            {
                // File included?
                if (fabricante.Imagem != null)
                {
                    // remove file
                    string filePath = Path.Combine(_imageFolder, fabricante.Imagem);
                    System.IO.File.Delete(filePath);

                }

                //Dependencies...?
                if (fabricante.Artigos.Count > 0)
                {
                    // Existem Artigos que referem esta fabricante, não pode apagar
                    throw new Exception("Existem Artigos que utilizam esta fabricante... não pode remover!");
                }

                _context
                    .fabricante
                    .Remove(fabricante);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // TODO: Handle failure
                return false;
            }

        }

        private string UploadedFile(fabricanteViewModel fabricanteVM)
        {
            string uniqueFileName = null;

            if (fabricanteVM.FicheiroImagem != null)
            {
                System.IO.Directory.CreateDirectory(_imageFolder);
                uniqueFileName = Guid.NewGuid().ToString()
                        + "_"
                        + System.IO.Path.GetFileName(fabricanteVM.FicheiroImagem.FileName);
                string filePath = Path.Combine(_imageFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fabricanteVM.FicheiroImagem.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }

}
