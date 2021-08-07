using BookSwap.Models;
using BookSwap.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.ViewComponents
{
    public class UserOnHeaderViewComponent : ViewComponent
    {
        private UserManager<User> userManager;

        public UserOnHeaderViewComponent(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string photoWay = await userManager.Users.Where(t => t.Id == userManager.GetUserId(UserClaimsPrincipal))
                .Select(t => t.PhotoWay).FirstOrDefaultAsync();
            return View(model: photoWay);
        }
    }
}
