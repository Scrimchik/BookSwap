using BookSwap.Models;
using BookSwap.Models.Users;
using BookSwap.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace BookSwap.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> roleManager;
        private ApplicationContext db;
        private IWebHostEnvironment webHostEnvironment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
            IWebHostEnvironment webHostEnvironment, ApplicationContext db, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            this.roleManager = roleManager;
            this.db = db;
        }

        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Name
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("GenreList", "Genre");
                }
            }
            return NoContent();
        }

        public async Task<IActionResult> CanRegistration(string name)
        {
            if (await _userManager.Users.AnyAsync(t => t.UserName == name))
                return Json(false);
            return Json(true);
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, true, false);

                if (result.Succeeded)
                    return RedirectToAction("GenreList", "Genre");
            }
            return NoContent();
        }

        public async Task<IActionResult> CanLogin(string name, string password)
        {
            User user = await _userManager.FindByNameAsync(name);
            if (password != null && user != null) 
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                return Json(result.Succeeded);
            }
            return Json(false);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("GenreList", "Genre");
        }

        public async Task<IActionResult> UserProfile(string userName)
        {
            User findedUser = await _userManager.FindByNameAsync(userName);
            ViewBag.CanChangePhoto = findedUser.Id == _userManager.GetUserId(User);
            return View(findedUser);
        }

        public async Task<IActionResult> ChangeImage(IFormFile image)
        {
            User user = await _userManager.GetUserAsync(User);
            if(user.PhotoWay != "default-user-image.jpg")
                DeletePastImage(user.PhotoWay);
            user.PhotoWay = image.FileName;
            await SaveImage(image);
            await db.SaveChangesAsync();
            return RedirectToAction("UserProfile", routeValues: new { userName = user.UserName });

        }

        private async Task SaveImage(IFormFile image)
        {
            string path = webHostEnvironment.WebRootPath + "/img/UsersPhotos/";

            using (var fileStream = new FileStream(path + image.FileName, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            using (Bitmap bitmap = new Bitmap(path + image.FileName))
            {
                Size bigSize = new Size(300, 300);
                using (Bitmap newBitmap = new Bitmap(bitmap, bigSize))
                {
                    newBitmap.Save(path + "Big/" + image.FileName);
                }

                Size smallSize = new Size(90, 90);
                using (Bitmap newBitmap = new Bitmap(bitmap, smallSize))
                {
                    newBitmap.Save(path + "Small/" + image.FileName);
                }
            }

            FileInfo file = new FileInfo(path + image.FileName);
            file.Delete();
        }

        private void DeletePastImage(string photoWay)
        {
            string path = webHostEnvironment.WebRootPath + "/img/UsersPhotos/";

            FileInfo pastSmallImage = new FileInfo(path + "Small/" + photoWay);
            FileInfo pastBigImage = new FileInfo(path + "Big/" + photoWay);
            pastBigImage.Delete();
            pastSmallImage.Delete();
        }
    }
}
