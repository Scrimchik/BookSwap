using BookSwap.Models.Abstraction;
using System.Linq;

namespace BookSwap.Models.EfRepository
{
    public class EfBookRepository : IBookRepository
    {
        private ApplicationContext db;

        public EfBookRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public IQueryable<Book> Books => db.Books;
    }
}
