using Microsoft.AspNetCore.Mvc;
using SegundaPracticaNetCore.Models;
using SegundaPracticaNetCore.Repositories;

namespace SegundaPracticaNetCore.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Details()
        {
            ViewData["COMICS"] = this.repo.GetComics();
            return View();
        }

        [HttpPost]
        public IActionResult Details(int id) 
        {
            Comic comic = this.repo.FindComic(id);
            ViewData["COMICS"] = this.repo.GetComics();
            return View(comic);
        }

        public IActionResult CreateProcedure()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProcedure(string nombre, string imagen, string descripcion)
        {
            this.repo.InsertarComicProcedure(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult CreateLambda()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLambda(string nombre, string imagen, string descripcion)
        {
            this.repo.InsertarComicLambda(nombre, imagen, descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Comic comic = this.repo.FindComic(id);
            return View(comic);
        }

        [HttpPost]
        public IActionResult DeleteConfirmation(int id)
        {
            this.repo.DeleteComic(id);
            return RedirectToAction("Index");
        }
    }
}
