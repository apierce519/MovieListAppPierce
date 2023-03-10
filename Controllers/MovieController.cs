using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieListAppPierce.Models;

namespace MovieListAppPierce.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext context;

        public MovieController(MovieContext cntx)
        {
            this.context = cntx;
        }

        public IActionResult Index()
        {
            var movies = context.Movies.Include(m => m.Genre)
            .OrderBy(m => m.Name).ToList();
            return View(movies);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Genres = context.Genres.OrderBy(g => g.Name).ToList();
            return View("Edit", new Movie());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Genres = context.Genres.OrderBy(g => g.Name).ToList();
            var movie = context.Movies.Find(id);
            return View(movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                if (movie.MovieId == 0)
                    context.Movies.Add(movie);
                else
                    context.Movies.Update(movie);
                context.SaveChanges();
                return RedirectToAction("Index", "Movie");
            }
            else
            {
                ViewBag.Action = (movie.MovieId == 0) ? "Add" : "Edit";
                return View(movie);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = context.Movies.Find(id);
            return View(movie);
        }
        [HttpPost]
        public IActionResult Delete(Movie movie)
        {
            context.Movies.Remove(movie);
            context.SaveChanges();
            return RedirectToAction("Index", "Movie");
        }
    }
}