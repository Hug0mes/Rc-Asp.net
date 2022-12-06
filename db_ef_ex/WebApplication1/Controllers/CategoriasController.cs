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
    public class CategoriasController : Controller
    {
        private readonly LojaContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IToastNotification _toastNotification;
        private readonly string _imageFolder;

        public CategoriasController(LojaContext context, IWebHostEnvironment appEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _toastNotification = toastNotification;
            _imageFolder = Path.Combine(_appEnvironment.WebRootPath, "images\\categorias");
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel CategoriaVM)
        {
            if (ModelState.IsValid)
            {
                await SaveCategoria(CategoriaVM);
                _toastNotification.AddSuccessToastMessage($"Categoria {CategoriaVM.Categoria.Nome} inserido com sucesso");
                return RedirectToAction(nameof(Index));
            }
            return View(CategoriaVM);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            CategoriaViewModel CategoriaVM = new CategoriaViewModel
            {
                Categoria = categoria
            };
            return View(CategoriaVM);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel CategoriaVM)
        {
            if (id != CategoriaVM.Categoria.Id)
            {
                return NotFound();
            }
            {
                if (ModelState.IsValid)
                {
                    await SaveCategoria(CategoriaVM);
                    _toastNotification.AddSuccessToastMessage($"Categoria {CategoriaVM.Categoria.Nome} alterada com sucesso");
                    return RedirectToAction(nameof(Index));
                }
                return View(CategoriaVM);
            }
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            await DeleteCategoria(categoria);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }


        // Refactor to Repository
        private async Task<bool> SaveCategoria(CategoriaViewModel CategoriaVM)
        {
            try
            {
                // File included?
                string uniqueFileName = UploadedFile(CategoriaVM);
                if ((uniqueFileName != null) || (CategoriaVM.RemoverImagem))
                {
                    // Previous image?
                    if (CategoriaVM.Categoria.Imagem != null)
                    {
                        // remove file
                        string filePath = Path.Combine(_imageFolder, CategoriaVM.Categoria.Imagem);
                        System.IO.File.Delete(filePath);

                    }
                    // Set filename image in Artigo
                    CategoriaVM.Categoria.Imagem = uniqueFileName;
                }

                // Save Categoria
                if (CategoriaVM.Categoria.Id == 0)
                {
                    //_context.Entry(Categoria.Categoria).State = EntityState.Added;
                    _context.Add(CategoriaVM.Categoria);
                }
                else
                {
                    //_context.Entry(Categoria.Categoria).State = EntityState.Modified;
                    _context.Update(CategoriaVM.Categoria);
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
        private async Task<bool> DeleteCategoria(Categoria Categoria)
        {
            try
            {
                // File included?
                if (Categoria.Imagem != null)
                {
                    // remove file
                    string filePath = Path.Combine(_imageFolder, Categoria.Imagem);
                    System.IO.File.Delete(filePath);

                }

                //Dependencies...?
                if (Categoria.Artigos.Count > 0)
                {
                    // Existem Artigos que referem esta categoria, não pode apagar
                    throw new Exception("Existem Artigos que utilizam esta Categoria... não pode remover!");
                }

                _context
                    .Categorias
                    .Remove(Categoria);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // TODO: Handle failure
                return false;
            }

        }

        private string UploadedFile(CategoriaViewModel CategoriaVM)
        {
            string uniqueFileName = null;

            if (CategoriaVM.FicheiroImagem != null)
            {
                System.IO.Directory.CreateDirectory(_imageFolder);
                uniqueFileName = Guid.NewGuid().ToString()
                        + "_"
                        + System.IO.Path.GetFileName(CategoriaVM.FicheiroImagem.FileName);
                string filePath = Path.Combine(_imageFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    CategoriaVM.FicheiroImagem.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }

}
