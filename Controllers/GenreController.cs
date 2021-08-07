using BookSwap.Models;
using BookSwap.Models.Abstraction;
using BookSwap.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Controllers
{
    public class GenreController : Controller
    {
        private IGenreRepository genreRepository;
        private ApplicationContext db;
        private IWebHostEnvironment webHostEnvironment;
        private int genrePerPage = 12;

        public GenreController(IGenreRepository genreRepository, ApplicationContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.genreRepository = genreRepository;
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> GenreList(int page = 1)
        {
            GenreListViewModel model = new GenreListViewModel
            {
                BookGenres = await genreRepository.Genres.OrderByDescending(t => t.Id).Skip(genrePerPage * (page - 1)).Take(genrePerPage).ToListAsync(),
                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = genrePerPage,
                    TotalItems = await genreRepository.Genres.CountAsync()
                }
            };
            return View(model);
        }

        public async Task<IActionResult> GenreAdmin()
        {
            return View(await genreRepository.Genres.ToListAsync());
        }

        public async Task<IActionResult> EditGenre(int genreId)
        {
            return View(await genreRepository.Genres.FirstOrDefaultAsync(t => t.Id == genreId));
        }

        [HttpPost]
        public async Task<IActionResult> EditGenre(BookGenre genre, IFormFile image)
        {
            BookGenre unupdatedGenre = await genreRepository.Genres.FirstOrDefaultAsync(t => t.Id == genre.Id);
            unupdatedGenre.Name = genre.Name;
            if (image != null)
            {
                DeleteGenrePhoto(unupdatedGenre.PhotoWay);
                unupdatedGenre.PhotoWay = await AddGenrePhoto(image, unupdatedGenre.LatinName);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("GenreAdmin");
        }

        public IActionResult AddGenre()
        {
            return View(new BookGenre());
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(BookGenre genre, IFormFile image)
        {
            genre.PhotoWay = await AddGenrePhoto(image, genre.LatinName);
            await db.BookGenres.AddAsync(genre);
            await db.SaveChangesAsync();
            Directory.CreateDirectory(webHostEnvironment.WebRootPath + "/img/BooksPhotos/" + genre.LatinName);

            return RedirectToAction("GenreAdmin");
        }

        public async Task<IActionResult> DeleteGenre(int genreId)
        {
            db.BookGenres.Remove(new BookGenre { Id = genreId });
            DeleteGenrePhoto(await genreRepository.Genres.Where(t => t.Id == genreId).Select(t => t.PhotoWay).FirstOrDefaultAsync());
            await db.SaveChangesAsync();

            return RedirectToAction("GenreAdmin");
        }

        private async Task<string> AddGenrePhoto(IFormFile image, string genreName)
        {
            if (image == null)
                return "default-img.jpg";

            string path = "/img/Genres/" + image.FileName;
            using (var fileStream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            string updatePath = "/img/Genres/" + genreName + ".jpg";
            using (Bitmap bitmap = new Bitmap(webHostEnvironment.WebRootPath + path))
            {
                Size size = new Size(300, 450);
                using (Bitmap newBitmap = new Bitmap(bitmap, size))
                {
                    newBitmap.Save(webHostEnvironment.WebRootPath + updatePath, ImageFormat.Jpeg);
                }
            }
            FileInfo file = new FileInfo(webHostEnvironment.WebRootPath + path);
            file.Delete();

            return genreName + ".jpg";
        }

        private void DeleteGenrePhoto(string imageName)
        {
            FileInfo image = new FileInfo(webHostEnvironment.WebRootPath + "/img/Genres/" + imageName);
            image.Delete();
        }
    }
}
