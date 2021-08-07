using BookSwap.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(t => t.UploadBooks).HasDefaultValue(0);
            builder.Entity<User>().Property(t => t.DownloadBooks).HasDefaultValue(0);
            builder.Entity<User>().Property(t => t.PhotoWay).HasDefaultValue("default-user-image.jpg");
            builder.Entity<Book>().Property(t => t.PhotoWay).HasDefaultValue("/img/BooksPhotos/default-img.jpg");
            builder.Entity<Book>().Property(t => t.DownloadCount).HasDefaultValue(0);
        }
    }
}
