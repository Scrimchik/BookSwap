using BookSwap.Models;
using BookSwap.Models.Abstraction;
using BookSwap.Models.EfRepository;
using BookSwap.Models.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookSwap
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient<IGenreRepository, EfGenreRepository>();
            services.AddTransient<IBookRepository, EfBookRepository>();
            services.AddTransient<ICommentRepository, EfCommentRepository>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }
            ).AddEntityFrameworkStores<ApplicationContext>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "/profile/{userName}",
                    defaults: new { controller = "Account", action = "UserProfile" }
                    );
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "/{genreName}",
                    defaults: new { controller = "Book", action = "BookList", page = 1 }
                    );
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "/{genreName}/Page-{page}",
                    defaults: new { controller = "Book", action = "BookList"}
                    );
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{controller=Genre}/Page-{page}",
                    defaults: new { controller = "Genre", action = "GenreList" }
                    );
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "/{genreName}/{bookName}—{bookId}",
                    defaults: new { controller = "Book", action = "Book" }
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Genre}/{action=GenreList}");
            });
        }
    }
}
