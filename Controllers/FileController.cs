using BookSwap.Models;
using BookSwap.Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Controllers
{
    public class FileController : Controller
    {
        private ApplicationContext db;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;


        public FileController(ApplicationContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            db = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> UserProfilePhotoUpload(IFormFile myFile)
        {
            if (myFile != null)
            {
                if (myFile.ContentType.Contains("image"))
                {
                    string path = "/img/UsersPhotos/" + myFile.FileName;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await myFile.CopyToAsync(fileStream);
                    }
                    User user = db.Users.Where(t => t.Id == _userManager.GetUserId(User)).FirstOrDefault();
                    user.PhotoWay = path;
                    db.SaveChanges();
                    return Redirect("~/Genres");
                }
                else
                {
                    return Content("Неверный формат файла");
                }
            }
            else
            {
                return Content("Вы не выбрали никакого изображения");
            }
        }
    }
}
