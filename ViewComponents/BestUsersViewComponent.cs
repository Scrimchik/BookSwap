using BookSwap.Models;
using BookSwap.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.ViewComponents
{
    public class BestUsersViewComponent : ViewComponent
    {
        private UserManager<User> userManager;

        public BestUsersViewComponent(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<User> bestUsers = await userManager.Users.OrderByDescending(t => t.UploadBooks).Take(3).ToListAsync();
            return View(bestUsers);
        }
    }
}
