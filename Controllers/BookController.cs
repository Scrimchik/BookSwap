using BookSwap.Models;
using BookSwap.Models.Abstraction;
using BookSwap.Models.Users;
using BookSwap.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository bookRepository;
        private IGenreRepository genreRepository;
        private IWebHostEnvironment webHostEnvironment;
        private ApplicationContext db;
        private UserManager<User> userManager;
        private int bookPerPage = 12;

        public BookController(IBookRepository bookRepository, IGenreRepository genreRepository, 
            IWebHostEnvironment webHostEnvironment, ApplicationContext db,
            UserManager<User> userManager)
        {
            this.bookRepository = bookRepository;
            this.genreRepository = genreRepository;
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
        }

        public async Task<IActionResult> BookList(string genreName, int page = 1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = await bookRepository.Books.Where(t => t.BooksGenre.LatinName == genreName).OrderByDescending(t => t.Id)
                .Skip(bookPerPage * (page - 1)).Take(bookPerPage)
                .Include(t => t.BooksGenre).ToListAsync(),
                GenreName = await genreRepository.Genres.Where(t => t.LatinName == genreName).Select(t => t.Name).FirstOrDefaultAsync(),
                GenreLatinName = genreName,
                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = bookPerPage,
                    TotalItems = await bookRepository.Books.CountAsync(t => t.BooksGenre.LatinName == genreName)
                }
            };

            return View(model);
        }

        public async Task<IActionResult> Book(string genreName, string bookName, int bookId)
        {
            BookViewModel model = new BookViewModel
            {
                Book = await bookRepository.Books.Include(t => t.User).Include(t => t.Comments).ThenInclude(t => t.User).FirstOrDefaultAsync(t => t.Id == bookId),
                CurrentUserId = userManager.GetUserId(User)
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> AddBook()
        {
            AddBookViewModel model = new AddBookViewModel()
            {
                BookGenres = await genreRepository.Genres.Select(t => t.Name).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBook(Book book, string genreName, IFormFile image, IFormFile bookFile)
        {
            if (ModelState.IsValid && bookFile != null && bookFile.ContentType == "application/pdf" && 
                (image == null || image.ContentType == "image/jpeg")) {

                BookGenre genre = await genreRepository.GetBookGenreByNameAsync(genreName);

                book.LatinName = MakeBookLatinName(book.Name);
                book.BookPath = await AddBookFile(bookFile, genre.LatinName);
                book.PhotoWay = await AddBookPhoto(image, genre.LatinName, book.Name);
                book.BooksGenreId = genre.Id;
                User user = await userManager.GetUserAsync(User);
                user.UploadBooks++;
                book.UserWhoUploadId = user.Id;

                await bookRepository.AddBookAsync(book);

                return RedirectToAction("Book", new { genreName = genreName, bookName = book.LatinName, bookId = book.Id});
            }

            AddBookViewModel model = new AddBookViewModel()
            {
                BookGenres = await genreRepository.Genres.Select(t => t.Name).ToListAsync(),
                Book = book
            };

            if (bookFile == null || bookFile.ContentType != "application/pdf")
                ModelState.AddModelError("", "Чтобы добавить книгу нужно загрузить файл книги в формате pdf");
            if (image != null && image.ContentType != "image/jpeg")
                ModelState.AddModelError("", "Изображение книги должно быть в формате jpg");

            return View(model);
        }

        public async Task<IActionResult> EditBook(int bookId)
        {
            return View(await bookRepository.GetBookAsync(bookId));
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(Book book, IFormFile bookImage)
        {
            User user = await userManager.GetUserAsync(User);
            if((book.UserWhoUploadId == user.Id || await userManager.IsInRoleAsync(user, "admin")) && ModelState.IsValid) {
                book.LatinName = MakeBookLatinName(book.Name);

                if(bookImage != null)
                {
                    DeletePastBookPhoto(book.PhotoWay);
                    string genreLatinName = await genreRepository.Genres.Where(t => t.Id == book.BooksGenreId)
                        .Select(t => t.LatinName).FirstOrDefaultAsync();
                    book.PhotoWay = await AddBookPhoto(bookImage, genreLatinName, book.Name);
                }

                await bookRepository.UpdateBookAsync(book);
                return RedirectToAction("Book", new { genreName = book.BooksGenre.LatinName, bookName = book.LatinName, bookId = book.Id});
            }
            return View(book);
        }

        public async Task<IActionResult> DeleteBook(int bookId)
        {
            Book book = await bookRepository.GetBookAsync(bookId);
            User user = await userManager.GetUserAsync(User);
            if((book.UserWhoUploadId == user.Id || await userManager.IsInRoleAsync(user, "admin")) && ModelState.IsValid)
            {
                user.UploadBooks--;
                DeletePastBookPhoto(book.PhotoWay);
                db.Books.Remove(book);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("GenreList", "Genre");
        }

        [Authorize]
        public async Task<IActionResult> DownloadBook(int bookId)
        {
            User user = await userManager.GetUserAsync(User);
            Book book = await bookRepository.Books.FirstOrDefaultAsync(t => t.Id == bookId);
            user.DownloadBooks++;
            book.DownloadCount++;
            await db.SaveChangesAsync();
            return PhysicalFile(webHostEnvironment.WebRootPath + book.BookPath, "application/pdf", book.LatinName);
        }

        private string MakeBookLatinName(string bookName)
        {
            string[] latLow = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "b", "e", "yu", "ya" };
            string[] rusLow = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            string[] fallbackCharacters = { "#", "@", "?", "=", ";", ",", "+", "&", "%", @"\", "|", "{", "}", "[", "]", "<", ">" };

            bookName = bookName.ToLower();
            for (int i = 0; i <= 32; i++)
            {
                bookName = bookName.Replace(rusLow[i], latLow[i]);
            }
            //remove all fallback symborls for normilized url
            for (int i = 0; i < fallbackCharacters.Length; i++)
            {
                bookName = bookName.Replace(fallbackCharacters[i], "");
            }

            bookName = bookName.Replace(' ', '-');
            return bookName;
        }

        private async Task<string> AddBookFile(IFormFile bookFile, string genreLatinName)
        {
            string path = "/BookFiles/" + genreLatinName + '/' + bookFile.FileName;
            using(var fileStream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
            {
                await bookFile.CopyToAsync(fileStream);
            }

            return path;
        }

        private async Task<string> AddBookPhoto(IFormFile image, string genreLatinName, string bookName)
        {
            if(image == null)
                return "/img/BooksPhotos/" + "default-img.jpg";

            string path = "/img/BooksPhotos/" + genreLatinName + '/' + image.FileName;
            using (var fileStream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            string updatePath = "/img/BooksPhotos/" + genreLatinName + '/' + bookName + ".jpg";
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

            return updatePath;
        }

        private void DeletePastBookPhoto(string photoWay)
        {
            if(photoWay != "/img/BooksPhotos/default-img.jpg") {
                FileInfo file = new FileInfo(webHostEnvironment.WebRootPath + photoWay);
                file.Delete();
            }
        }
    }
}
