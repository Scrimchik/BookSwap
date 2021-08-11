using BookSwap.Models.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Models.EfRepository
{
    class EfGenreRepository : IGenreRepository
    {
        private ApplicationContext db;

        public EfGenreRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public IQueryable<BookGenre> Genres => db.BookGenres;

        public async Task AddGenreAsync(BookGenre genre)
        {
            db.BookGenres.Add(genre);
            await db.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(BookGenre genre)
        {
            db.BookGenres.Remove(genre);
            await db.SaveChangesAsync();
        }

        public async Task<BookGenre> GetBookGenreAsync(int id)
        {
            return await db.BookGenres.FindAsync(id);
        }

        public async Task<BookGenre> GetBookGenreByNameAsync(string latinName)
        {
            return await db.BookGenres.FirstOrDefaultAsync(t => t.LatinName == latinName);
        }

        public async Task UpdateGenreAsync(BookGenre genre)
        {
            BookGenre unupdatedGenre = await GetBookGenreAsync(genre.Id);
            unupdatedGenre.Name = genre.Name;
            unupdatedGenre.PhotoWay = genre.PhotoWay;
            await db.SaveChangesAsync();
        }
    }
}
