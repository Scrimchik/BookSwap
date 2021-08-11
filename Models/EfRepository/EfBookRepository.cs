using BookSwap.Models.Abstraction;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Book> GetBookAsync(int bookId)
        {
            return await db.Books.FindAsync(bookId);
        }

        public async Task AddBookAsync(Book book)
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            Book ununpdatedBook = await GetBookAsync(book.Id);
            ununpdatedBook.Name = book.Name;
            ununpdatedBook.LatinName = book.LatinName;
            ununpdatedBook.Description = book.Description;
            ununpdatedBook.Author = book.Author;
            ununpdatedBook.PhotoWay = book.PhotoWay;
            await db.SaveChangesAsync();
        }

        public Task DeleteBookAsync(Book book)
        {
            db.Books.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}
