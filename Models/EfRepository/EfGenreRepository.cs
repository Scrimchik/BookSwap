using BookSwap.Models.Abstraction;
using System.Linq;

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
    }
}
