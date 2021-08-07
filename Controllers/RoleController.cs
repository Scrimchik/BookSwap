using BookSwap.Models;
using BookSwap.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        private ApplicationContext db;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            db = context;
        }

        public async Task<IActionResult> Create()
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("admin"));
            await _roleManager.CreateAsync(new IdentityRole("moderator"));
            await _roleManager.CreateAsync(new IdentityRole("moder"));
            await _roleManager.CreateAsync(new IdentityRole("mod"));
            User user = db.Users.FirstOrDefault(t => t.UserName == "scrim");
            await _userManager.AddToRoleAsync(user, "admin");
            await _userManager.AddToRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "moderator");
            await _userManager.AddToRoleAsync(user, "moder");
            await _userManager.AddToRoleAsync(user, "mod");
            return StatusCode(200);
        }

    }
}
