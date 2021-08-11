using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Models.Abstraction
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }

        Task<Book> GetBookAsync(int bookId);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Book book);
    }
}
