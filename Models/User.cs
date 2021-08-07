using Microsoft.AspNetCore.Identity;

namespace BookSwap.Models.Users
{
    public class User : IdentityUser
    {
        public string PhotoWay { get; set; }
        public int UploadBooks { get; set; }
        public int DownloadBooks { get; set; } 
    }
}
